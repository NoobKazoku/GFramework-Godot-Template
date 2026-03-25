using Godot;

namespace GFrameworkGodotTemplate.scripts.core.localization;

/// <summary>
///     本地化 RichTextLabel 组件
///     自动根据语言变化更新富文本内容，支持 BBCode
/// </summary>
[GlobalClass]
[ContextAware]
public partial class LocalizationRichTextLabel : RichTextLabel
{
    private LocalizationTextController? _textController;

    /// <summary>
    ///     本地化表名
    /// </summary>
    [Export]
    public string LocalizationTable { get; set; } = "common";

    /// <summary>
    ///     本地化键名
    /// </summary>
    [Export]
    public string LocalizationKey { get; set; } = string.Empty;

    /// <summary>
    ///     是否在 Ready 时自动更新文本
    /// </summary>
    [Export]
    public bool AutoUpdate { get; set; } = true;

    /// <summary>
    ///     是否启用 BBCode
    /// </summary>
    [Export]
    public bool EnableBbCode { get; set; } = true;

    public override void _Ready()
    {
        // 设置 BBCode 启用状态
        BbcodeEnabled = EnableBbCode;
        EnsureTextController().Ready(AutoUpdate);
    }

    public override void _ExitTree()
    {
        _textController?.ExitTree();
    }

    /// <summary>
    ///     设置变量
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void SetVariable(string name, object value)
    {
        EnsureTextController().SetVariable(name, value);
    }

    /// <summary>
    ///     批量设置变量
    /// </summary>
    /// <param name="variables">变量字典</param>
    public void SetVariables(IReadOnlyDictionary<string, object> variables)
    {
        EnsureTextController().SetVariables(variables);
    }

    /// <summary>
    ///     清除所有变量
    /// </summary>
    public void ClearVariables()
    {
        EnsureTextController().ClearVariables();
    }

    /// <summary>
    ///     更新文本
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