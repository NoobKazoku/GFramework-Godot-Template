using GFramework.Core.Cqrs.Command;
using GFramework.Game.Abstractions.Setting;
using GFramework.Game.Abstractions.Setting.Data;
using GFramework.Godot.Setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改音效音量命令处理器
/// </summary>
public class ChangeSfxVolumeCommandHandler : AbstractCommandHandler<ChangeSfxVolumeCommand>
{
    private ISettingsModel? _model;
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(ChangeSfxVolumeCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        (_model ??= this.GetModel<ISettingsModel>()!).GetData<AudioSettings>().SfxVolume = input.Volume;
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.Apply<GodotAudioSettings>().ConfigureAwait(true);
        return Unit.Value;
    }
}