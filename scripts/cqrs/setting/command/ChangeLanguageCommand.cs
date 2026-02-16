using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.setting.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.command;

/// <summary>
///     更改语言命令类
/// </summary>
/// <param name="input">语言更改命令输入参数</param>
public sealed class ChangeLanguageCommand(ChangeLanguageCommandInput input)
    : CommandBase<ChangeLanguageCommandInput, Unit>(input);