using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.audio.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改主音量命令类
/// </summary>
/// <param name="input">主音量更改命令输入参数</param>
public sealed class ChangeMasterVolumeCommand(ChangeMasterVolumeCommandInput input)
    : CommandBase<ChangeMasterVolumeCommandInput, Unit>(input);