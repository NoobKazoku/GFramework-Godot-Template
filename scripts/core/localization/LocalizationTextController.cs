namespace GFrameworkGodotTemplate.scripts.core.localization;

/// <summary>
///     共享本地化文本更新逻辑，统一处理变量、订阅与安全刷新。
/// </summary>
internal sealed class LocalizationTextController(
    Func<ILocalizationManager?> resolveLocalizationManager,
    Func<string> getLocalizationTable,
    Func<string> getLocalizationKey,
    Action<string> applyText)
{
    private readonly Dictionary<string, object> _variables = new(StringComparer.OrdinalIgnoreCase);
    private ILocalizationManager? _locManager;
    private bool _subscribed;

    public void Ready(bool autoUpdate)
    {
        SubscribeToLanguageChange();

        if (autoUpdate) UpdateText();
    }

    public void ExitTree()
    {
        UnsubscribeFromLanguageChange();
    }

    public void SetVariable(string name, object value)
    {
        _variables[name] = value;
        UpdateText();
    }

    public void SetVariables(IReadOnlyDictionary<string, object> variables)
    {
        foreach (var (name, value) in variables) _variables[name] = value;

        UpdateText();
    }

    public void ClearVariables()
    {
        _variables.Clear();
        UpdateText();
    }

    public void UpdateText()
    {
        var localizationKey = getLocalizationKey();
        if (string.IsNullOrEmpty(localizationKey)) return;

        var locManager = ResolveLocalizationManager();
        if (locManager is null) return;

        var locString = locManager.GetString(getLocalizationTable(), localizationKey);
        if (locString is null) return;

        foreach (var (name, value) in _variables) locString.WithVariable(name, value);

        applyText(locString.Format());
    }

    private void SubscribeToLanguageChange()
    {
        if (_subscribed) return;

        var locManager = ResolveLocalizationManager();
        if (locManager is null) return;

        locManager.SubscribeToLanguageChange(OnLanguageChanged);
        _subscribed = true;
    }

    private void UnsubscribeFromLanguageChange()
    {
        if (!_subscribed || _locManager is null) return;

        _locManager.UnsubscribeFromLanguageChange(OnLanguageChanged);
        _subscribed = false;
    }

    private ILocalizationManager? ResolveLocalizationManager()
    {
        return _locManager ??= resolveLocalizationManager();
    }

    private void OnLanguageChanged(string language)
    {
        UpdateText();
    }
}