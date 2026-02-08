using GFramework.Core.Abstractions.command;

namespace GFrameworkGodotTemplate.scripts.command.setting.input;

/// <summary>
///     更改语言命令输入类
///     用于封装更改语言的命令输入参数
/// </summary>
public sealed class ChangeLanguageCommandInput : ICommandInput
{
    /// <summary>
    ///     获取或设置要更改的新语言
    /// </summary>
    public string Language { get; init; } = null!;
}