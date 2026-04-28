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
        if (string.IsNullOrWhiteSpace(locale)) return GetRuntimeProfile().DefaultLanguageId;

        var normalized = locale.Replace("_", "-", StringComparison.Ordinal).ToLowerInvariant();
        return normalized.StartsWith("zh", StringComparison.Ordinal) ? "zh-cn" : "en";
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

    private TConfig ResolveByLanguage<TConfig>(IConfigTable<string, TConfig> table)
    {
        var languageId = GetCurrentLanguageId();
        if (table.TryGet(languageId, out var config) && config is not null) return config;

        return table.Get("en");
    }
}
