using System.IO;
using System.Reflection;
using Godot;
using FileAccess = Godot.FileAccess;

namespace GFrameworkGodotTemplate.scripts.config;

/// <summary>
///     Synchronizes bundled YAML and schema files to a runtime-readable cache.
/// </summary>
public static class TemplateBundledConfigCache
{
    private static readonly string[] BundledConfigFiles =
    [
        "config/common_text/en.yaml",
        "config/common_text/zh-cn.yaml",
        "config/menu_text/en.yaml",
        "config/menu_text/zh-cn.yaml",
        "config/runtime_profile/default.yaml",
        "schemas/common_text.schema.json",
        "schemas/menu_text.schema.json",
        "schemas/runtime_profile.schema.json"
    ];

    public static bool CanUseDirectSourceDirectory(string sourceRootPath)
    {
        if (sourceRootPath.StartsWith("res://", StringComparison.Ordinal) && !OS.HasFeature("editor"))
            return false;

        foreach (var directoryPath in BundledConfigFiles
                     .Where(static relativePath => relativePath.StartsWith("config/", StringComparison.Ordinal))
                     .Select(static relativePath => Path.GetDirectoryName(relativePath))
                     .Where(static path => !string.IsNullOrWhiteSpace(path))
                     .Distinct(StringComparer.Ordinal))
        {
            var fullPath = ToAbsolutePath(TemplateContentPathResolver.CombinePath(sourceRootPath, directoryPath!));
            if (!Directory.Exists(fullPath)) return false;
        }

        foreach (var schemaPath in BundledConfigFiles
                     .Where(static relativePath => relativePath.StartsWith("schemas/", StringComparison.Ordinal)))
        {
            var fullPath = ToAbsolutePath(TemplateContentPathResolver.CombinePath(sourceRootPath, schemaPath));
            if (!File.Exists(fullPath)) return false;
        }

        return true;
    }

    public static void SynchronizeToCache(string sourceRootPath, string cacheRootPath)
    {
        var cacheRootAbsolutePath = ToAbsolutePath(cacheRootPath);
        var synchronizedFiles = new Dictionary<string, byte[]>(StringComparer.Ordinal);

        foreach (var relativePath in BundledConfigFiles)
        {
            var sourcePath = TemplateContentPathResolver.CombinePath(sourceRootPath, relativePath);
            if (!TryReadBundledFileBytes(sourcePath, relativePath, out var bytes))
                throw new FileNotFoundException(
                    $"Bundled config file '{sourcePath}' was not found while synchronizing the runtime cache.",
                    sourcePath);

            synchronizedFiles.Add(relativePath, bytes);
        }

        var temporaryCacheRootPath = $"{cacheRootAbsolutePath}.tmp-{Guid.NewGuid():N}";
        try
        {
            Directory.CreateDirectory(temporaryCacheRootPath);

            foreach (var (relativePath, bytes) in synchronizedFiles)
            {
                var targetPath = Path.Combine(
                    temporaryCacheRootPath,
                    relativePath.Replace('/', Path.DirectorySeparatorChar));
                var parentDirectory = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrWhiteSpace(parentDirectory)) Directory.CreateDirectory(parentDirectory);

                File.WriteAllBytes(targetPath, bytes);
            }

            if (Directory.Exists(cacheRootAbsolutePath)) Directory.Delete(cacheRootAbsolutePath, true);
            Directory.Move(temporaryCacheRootPath, cacheRootAbsolutePath);
        }
        finally
        {
            if (Directory.Exists(temporaryCacheRootPath)) Directory.Delete(temporaryCacheRootPath, true);
        }
    }

    private static bool TryReadBundledFileBytes(string sourcePath, string relativePath, out byte[] bytes)
    {
        if (FileAccess.FileExists(sourcePath))
        {
            bytes = FileAccess.GetFileAsBytes(sourcePath);
            return true;
        }

        var resourceName = FindEmbeddedResourceName(relativePath);
        if (resourceName == null)
        {
            bytes = [];
            return false;
        }

        using var stream = typeof(TemplateBundledConfigCache).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            bytes = [];
            return false;
        }

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
        return true;
    }

    private static string? FindEmbeddedResourceName(string relativePath)
    {
        var assembly = typeof(TemplateBundledConfigCache).Assembly;
        var manifestResourceNames = assembly.GetManifestResourceNames();
        foreach (var candidate in GetManifestResourceNameCandidates(relativePath, assembly))
        {
            var exactMatch = manifestResourceNames.FirstOrDefault(resourceName =>
                string.Equals(resourceName, candidate, StringComparison.OrdinalIgnoreCase));
            if (exactMatch != null) return exactMatch;
        }

        foreach (var candidate in GetManifestResourceNameCandidates(relativePath, assembly))
        {
            var suffixMatch = manifestResourceNames.FirstOrDefault(resourceName =>
                resourceName.EndsWith(candidate, StringComparison.OrdinalIgnoreCase));
            if (suffixMatch != null) return suffixMatch;
        }

        return null;
    }

    private static IEnumerable<string> GetManifestResourceNameCandidates(string relativePath, Assembly assembly)
    {
        var normalizedSlashPath = relativePath.Replace('\\', '/').TrimStart('/');
        yield return normalizedSlashPath;

        var normalizedBackslashPath = normalizedSlashPath.Replace('/', '\\');
        if (!string.Equals(normalizedBackslashPath, normalizedSlashPath, StringComparison.Ordinal))
            yield return normalizedBackslashPath;

        var normalizedDotPath = normalizedSlashPath.Replace('/', '.');
        if (!string.Equals(normalizedDotPath, normalizedSlashPath, StringComparison.Ordinal))
            yield return normalizedDotPath;

        var assemblyName = assembly.GetName().Name;
        if (!string.IsNullOrWhiteSpace(assemblyName)) yield return $"{assemblyName}.{normalizedDotPath}";
    }

    private static string ToAbsolutePath(string path)
    {
        if (path.StartsWith("res://", StringComparison.Ordinal))
        {
            if (!OS.HasFeature("editor"))
                throw new InvalidOperationException(
                    $"Godot path '{path}' cannot be converted to a native filesystem path outside the editor.");

            return TemplateContentPathResolver.GetAbsolutePath(path);
        }

        if (path.StartsWith("user://", StringComparison.Ordinal))
            return TemplateContentPathResolver.GetAbsolutePath(path);

        return Path.GetFullPath(path);
    }
}
