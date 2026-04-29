using System.IO;
using Godot;

namespace GFrameworkGodotTemplate.scripts.config;

/// <summary>
///     Provides Godot project-setting paths for template content configuration.
/// </summary>
public static class TemplateContentPathResolver
{
    public const string SourceRootSettingKey = "application/config/content/source_root_path";
    public const string CacheRootSettingKey = "application/config/content/cache_root_path";
    public const string MenuTextProjectDirectorySettingKey = "application/config/content/menu_text_project_directory_path";
    public const string CommonTextProjectDirectorySettingKey =
        "application/config/content/common_text_project_directory_path";
    public const string RuntimeProfileProjectDirectorySettingKey =
        "application/config/content/runtime_profile_project_directory_path";

    public static bool CanMutateProjectConfig => OS.HasFeature("editor");

    public static string GetConfiguredSourceRootPath()
    {
        return GetConfiguredPath(SourceRootSettingKey);
    }

    public static string GetConfiguredCacheRootPath()
    {
        return GetConfiguredPath(CacheRootSettingKey);
    }

    public static string GetConfiguredMenuTextProjectDirectoryPath()
    {
        return GetConfiguredPath(MenuTextProjectDirectorySettingKey);
    }

    public static string GetConfiguredCommonTextProjectDirectoryPath()
    {
        return GetConfiguredPath(CommonTextProjectDirectorySettingKey);
    }

    public static string GetConfiguredRuntimeProfileProjectDirectoryPath()
    {
        return GetConfiguredPath(RuntimeProfileProjectDirectorySettingKey);
    }

    public static string GetConfiguredSchemaValidationRootPath()
    {
        var sourceRootPath = GetConfiguredSourceRootPath();
        return TemplateBundledConfigCache.CanUseDirectSourceDirectory(sourceRootPath)
            ? GetAbsolutePath(sourceRootPath)
            : GetAbsolutePath(GetConfiguredCacheRootPath());
    }

    public static string BuildMenuTextConfigFilePath(string languageId)
    {
        var safeLanguageId = ValidateConfigIdentifier(languageId, nameof(languageId));
        return CombinePath(GetConfiguredMenuTextProjectDirectoryPath(), $"{safeLanguageId}.yaml");
    }

    public static string BuildCommonTextConfigFilePath(string languageId)
    {
        var safeLanguageId = ValidateConfigIdentifier(languageId, nameof(languageId));
        return CombinePath(GetConfiguredCommonTextProjectDirectoryPath(), $"{safeLanguageId}.yaml");
    }

    public static string BuildRuntimeProfileConfigFilePath(string profileId)
    {
        var safeProfileId = ValidateConfigIdentifier(profileId, nameof(profileId));
        return CombinePath(GetConfiguredRuntimeProfileProjectDirectoryPath(), $"{safeProfileId}.yaml");
    }

    public static string GetAbsolutePath(string path)
    {
        var normalizedPath = NormalizePath(path);
        if (normalizedPath.StartsWith("res://", StringComparison.Ordinal) ||
            normalizedPath.StartsWith("user://", StringComparison.Ordinal))
        {
            var absolutePath = ProjectSettings.GlobalizePath(normalizedPath);
            if (string.IsNullOrWhiteSpace(absolutePath))
                throw new InvalidOperationException($"Failed to globalize Godot path '{normalizedPath}'.");

            return absolutePath;
        }

        return Path.GetFullPath(normalizedPath);
    }

    private static string GetConfiguredPath(string settingKey)
    {
        if (!ProjectSettings.HasSetting(settingKey))
            throw new InvalidOperationException($"Project setting '{settingKey}' is required but missing.");

        var path = ProjectSettings.GetSetting(settingKey).AsString();
        if (string.IsNullOrWhiteSpace(path))
            throw new InvalidOperationException($"Project setting '{settingKey}' cannot be empty or whitespace.");

        return NormalizePath(path);
    }

    internal static string CombinePath(string rootPath, string relativePath)
    {
        var normalizedRoot = NormalizePath(rootPath);
        var normalizedRelativePath = relativePath.Replace('\\', '/').Trim('/');
        return string.IsNullOrEmpty(normalizedRelativePath)
            ? normalizedRoot
            : $"{normalizedRoot}/{normalizedRelativePath}";
    }

    private static string ValidateConfigIdentifier(string value, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        var normalizedValue = value.Trim();
        if (Path.IsPathRooted(normalizedValue) ||
            normalizedValue.Contains("..", StringComparison.Ordinal) ||
            normalizedValue.IndexOfAny(['/', '\\', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar]) >= 0 ||
            normalizedValue.Contains(Path.PathSeparator) ||
            normalizedValue.Contains(':'))
        {
            throw new ArgumentException("Identifier contains invalid path characters.", parameterName);
        }

        return normalizedValue;
    }

    private static string NormalizePath(string path)
    {
        var normalized = path.Trim().Replace('\\', '/');
        if (normalized is "res://" or "user://") return normalized;

        if (normalized.Length == 3 &&
            char.IsLetter(normalized[0]) &&
            normalized[1] == ':' &&
            normalized[2] == '/')
            return normalized;

        return normalized.TrimEnd('/');
    }
}
