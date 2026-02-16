using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.graphics.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     更改分辨率命令类
/// </summary>
/// <param name="input">分辨率更改命令输入参数</param>
public sealed class ChangeResolutionCommand(ChangeResolutionCommandInput input)
    : CommandBase<ChangeResolutionCommandInput, Unit>(input);