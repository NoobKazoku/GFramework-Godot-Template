namespace GFrameworkGodotTemplate.scripts.config;

/// <summary>
///     Owns the template process configuration registry lifecycle.
/// </summary>
public sealed class TemplateConfigHost : IDisposable
{
    private static readonly GeneratedConfigRegistrationOptions RegistrationOptions = new()
    {
        CommonTextComparer = StringComparer.OrdinalIgnoreCase,
        RuntimeProfileComparer = StringComparer.OrdinalIgnoreCase,
        MenuTextComparer = StringComparer.OrdinalIgnoreCase
    };

    private readonly Lock _gate = new();
    private bool _disposed;
    private IConfigRegistry _registry;

    public TemplateConfigHost()
    {
        _registry = CreateAndInitializeRegistry();
    }

    public IConfigRegistry Registry
    {
        get
        {
            lock (_gate)
            {
                ThrowIfDisposed();
                return _registry;
            }
        }
    }

    public void Reload()
    {
        var nextRegistry = CreateAndInitializeRegistry();
        IConfigRegistry previousRegistry;

        lock (_gate)
        {
            if (_disposed)
            {
                if (nextRegistry is IDisposable disposableRegistry) disposableRegistry.Dispose();
                ThrowIfDisposed();
            }

            previousRegistry = _registry;
            _registry = nextRegistry;
        }

        if (previousRegistry is IDisposable disposablePreviousRegistry) disposablePreviousRegistry.Dispose();
    }

    public void Dispose()
    {
        IConfigRegistry? registryToDispose = null;

        lock (_gate)
        {
            if (_disposed) return;
            _disposed = true;
            registryToDispose = _registry;
        }

        if (registryToDispose is IDisposable disposableRegistry) disposableRegistry.Dispose();
    }

    private static IReadOnlyCollection<GodotYamlConfigTableSource> CreateTableSources()
    {
        return GeneratedConfigCatalog
            .GetTablesForRegistration(RegistrationOptions)
            .Select(static metadata => new GodotYamlConfigTableSource(
                metadata.TableName,
                metadata.ConfigRelativePath,
                metadata.SchemaRelativePath))
            .ToArray();
    }

    private static IConfigRegistry CreateAndInitializeRegistry()
    {
        var registry = new ConfigRegistry();
        var sourceRootPath = TemplateContentPathResolver.GetConfiguredSourceRootPath();
        var cacheRootPath = TemplateContentPathResolver.GetConfiguredCacheRootPath();
        var loaderSourceRootPath = sourceRootPath;
        if (!TemplateBundledConfigCache.CanUseDirectSourceDirectory(sourceRootPath))
        {
            TemplateBundledConfigCache.SynchronizeToCache(sourceRootPath, cacheRootPath);
            loaderSourceRootPath = cacheRootPath;
        }

        var loader = new GodotYamlConfigLoader(
            new GodotYamlConfigLoaderOptions
            {
                SourceRootPath = loaderSourceRootPath,
                RuntimeCacheRootPath = cacheRootPath,
                TableSources = CreateTableSources(),
                ConfigureLoader = static yamlLoader => yamlLoader.RegisterAllGeneratedConfigTables(RegistrationOptions)
            });

        Task.Run(() => loader.LoadAsync(registry)).GetAwaiter().GetResult();
        return registry;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(TemplateConfigHost));
    }
}
