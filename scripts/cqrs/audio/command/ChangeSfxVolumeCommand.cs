using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.audio.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改音效音量命令类
/// </summary>
/// <param name="input">音效音量更改命令输入参数</param>
public sealed class ChangeSfxVolumeCommand(ChangeSfxVolumeCommandInput input)
    : CommandBase<ChangeSfxVolumeCommandInput, Unit>(input);