using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.audio.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改背景音乐音量命令类
/// </summary>
/// <param name="input">背景音乐音量更改命令输入参数</param>
public sealed class ChangeBgmVolumeCommand(ChangeBgmVolumeCommandInput input)
    : CommandBase<ChangeBgmVolumeCommandInput, Unit>(input);