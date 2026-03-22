using GFramework.Core.Abstractions.Localization;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.localization;

/// <summary>
/// 本地化 Button 组件
/// 自动根据语言变化更新按钮文本
/// </summary>
[GlobalClass]
[ContextAware]
public partial class LocalizationButton : Button
{
    private readonly Dictionary<string, object> _variables = new(StringComparer.OrdinalIgnoreCase);
    private ILocalizationManager _locManager = null!;
    private ILocalizationString? _locString;
    private bool _subscribed;

    /// <summary>
    /// 本地化表名
    /// </summary>
    [Export]
    public string LocalizationTable { get; set; } = "common";

    /// <summary>
    /// 本地化键名
    /// </summary>
    [Export]
    public string LocalizationKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否在 Ready 时自动更新文本
    /// </summary>
    [Export]
    public bool AutoUpdate { get; set; } = true;

    /// <summary>
    /// 组件就绪时的初始化方法
    /// 初始化本地化管理器并订阅语言变化事件
    /// 如果启用自动更新，则尝试初始化文本
    /// </summary>
    public override void _Ready()
    {
        // 从架构中获取本地化管理器
        _locManager = this.GetSystem<ILocalizationManager>()!;

        if (!_subscribed)
        {
            // 订阅语言变化事件
            _locManager.SubscribeToLanguageChange(OnLanguageChanged);
            _subscribed = true;
        }

        // 如果启用自动更新，尝试初始化
        if (AutoUpdate)
        {
            UpdateText();
        }
    }

    /// <summary>
    /// 组件退出场景树时的清理方法
    /// 取消订阅语言变化事件以避免内存泄漏
    /// </summary>
    public override void _ExitTree()
    {
        // 取消订阅
        UnsubscribeFromLanguageChange();
    }

    /// <summary>
    /// 设置单个变量
    /// 将指定名称的变量添加到变量字典中，并触发文本更新
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void SetVariable(string name, object value)
    {
        _variables[name] = value;
        UpdateText();
    }

    /// <summary>
    /// 批量设置变量
    /// 将提供的变量字典中的所有变量添加到内部变量集合中，并触发文本更新
    /// </summary>
    /// <param name="variables">包含变量名值对的只读字典</param>
    public void SetVariables(IReadOnlyDictionary<string, object> variables)
    {
        foreach (var (name, value) in variables)
        {
            _variables[name] = value;
        }

        UpdateText();
    }

    /// <summary>
    /// 清除所有已设置的变量
    /// 清空内部变量字典并触发文本更新
    /// </summary>
    public void ClearVariables()
    {
        _variables.Clear();
        UpdateText();
    }

    /// <summary>
    /// 更新显示文本
    /// 根据当前的本地化表名和键名获取对应的本地化字符串，
    /// 应用所有已设置的变量，并格式化后设置为Button的文本内容
    /// </summary>
    public void UpdateText()
    {
        if (string.IsNullOrEmpty(LocalizationKey))
        {
            return;
        }

        // 获取本地化字符串
        _locString = _locManager.GetString(LocalizationTable, LocalizationKey);

        // 应用变量
        foreach (var (name, value) in _variables)
        {
            _locString.WithVariable(name, value);
        }

        // 格式化并设置文本
        Text = _locString.Format();
    }

    /// <summary>
    /// 取消订阅语言变化事件
    /// 安全地取消事件订阅，检查管理器是否为空并防止重复取消
    /// </summary>
    private void UnsubscribeFromLanguageChange()
    {
        if (_subscribed)
        {
            _locManager.UnsubscribeFromLanguageChange(OnLanguageChanged);
            _subscribed = false;
        }
    }

    /// <summary>
    /// 语言变化回调方法
    /// 当系统语言发生变化时被调用，触发文本更新
    /// </summary>
    /// <param name="language">新的语言代码</param>
    private void OnLanguageChanged(string language)
    {
        UpdateText();
    }
}