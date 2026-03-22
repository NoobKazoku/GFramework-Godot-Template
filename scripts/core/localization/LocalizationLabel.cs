using GFramework.Core.Abstractions.Localization;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.localization;

/// <summary>
/// 本地化 Label 组件
/// 自动根据语言变化更新文本
/// </summary>
[GlobalClass]
[ContextAware]
public partial class LocalizationLabel : Label
{
    private LocalizationTextController? _textController;

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
        EnsureTextController().Ready(AutoUpdate);
    }

    /// <summary>
    /// 组件退出场景树时的清理方法
    /// 取消订阅语言变化事件以避免内存泄漏
    /// </summary>
    public override void _ExitTree()
    {
        _textController?.ExitTree();
    }

    /// <summary>
    /// 设置单个变量
    /// 将指定名称的变量添加到变量字典中，并触发文本更新
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void SetVariable(string name, object value)
    {
        EnsureTextController().SetVariable(name, value);
    }

    /// <summary>
    /// 批量设置变量
    /// 将提供的变量字典中的所有变量添加到内部变量集合中，并触发文本更新
    /// </summary>
    /// <param name="variables">包含变量名值对的只读字典</param>
    public void SetVariables(IReadOnlyDictionary<string, object> variables)
    {
        EnsureTextController().SetVariables(variables);
    }

    /// <summary>
    /// 清除所有已设置的变量
    /// 清空内部变量字典并触发文本更新
    /// </summary>
    public void ClearVariables()
    {
        EnsureTextController().ClearVariables();
    }

    /// <summary>
    /// 更新显示文本
    /// 根据当前的本地化表名和键名获取对应的本地化字符串，
    /// 应用所有已设置的变量，并格式化后设置为Label的文本内容
    /// </summary>
    public void UpdateText()
    {
        EnsureTextController().UpdateText();
    }

    private LocalizationTextController EnsureTextController()
    {
        return _textController ??= new LocalizationTextController(
            () => this.GetSystem<ILocalizationManager>(),
            () => LocalizationTable,
            () => LocalizationKey,
            text => Text = text);
    }
}