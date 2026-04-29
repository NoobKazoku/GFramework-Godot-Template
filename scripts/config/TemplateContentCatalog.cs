using Godot;

namespace GFrameworkGodotTemplate.scripts.config;

/// <summary>
///     Loads and exposes static template content configuration.
/// </summary>
public sealed class TemplateContentCatalog : ITemplateContentCatalog
{
    private CommonTextTable _commonTextTable = null!;
    private readonly TemplateConfigHost _configHost;
    private MenuTextTable _menuTextTable = null!;
    private RuntimeProfileTable _runtimeProfileTable = null!;

    public TemplateContentCatalog()
    {
        _configHost = new TemplateConfigHost();
        RefreshReloadableTables(_configHost.Registry);
    }

    public CommonTextConfig GetCommonText()
    {
        return ResolveByLanguage(_commonTextTable);
    }

    public MenuTextConfig GetMenuText()
    {
        return ResolveByLanguage(_menuTextTable);
    }

    public RuntimeProfileConfig GetRuntimeProfile()
    {
        return _runtimeProfileTable.Get("default");
    }

    public string GetCurrentLanguageId()
    {
        var locale = TranslationServer.GetLocale();
        var fallbackLanguageId = GetFallbackLanguageId();
        if (string.IsNullOrWhiteSpace(locale)) return fallbackLanguageId;

        var normalized = locale.Replace("_", "-", StringComparison.Ordinal).ToLowerInvariant();
        if (normalized.StartsWith("zh", StringComparison.Ordinal)) return "zh-cn";
        if (normalized.StartsWith("en", StringComparison.Ordinal)) return "en";

        return fallbackLanguageId;
    }

    public void Reload()
    {
        _configHost.Reload();
        RefreshReloadableTables(_configHost.Registry);
    }

    private void RefreshReloadableTables(IConfigRegistry registry)
    {
        _commonTextTable = registry.GetCommonTextTable();
        _menuTextTable = registry.GetMenuTextTable();
        _runtimeProfileTable = registry.GetRuntimeProfileTable();
    }

    private string GetFallbackLanguageId()
    {
        var fallbackLanguageId = GetRuntimeProfile().DefaultLanguageId;
        return string.IsNullOrWhiteSpace(fallbackLanguageId) ? "en" : fallbackLanguageId;
    }

    private TConfig ResolveByLanguage<TConfig>(IConfigTable<string, TConfig> table)
    {
        var languageId = GetCurrentLanguageId();
        if (table.TryGet(languageId, out var config) && config is not null) return config;

        return table.Get("en");
    }
}
