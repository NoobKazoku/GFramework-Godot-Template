using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改主音量命令处理器
/// </summary>
public class ChangeMasterVolumeCommandHandler : AbstractCommandHandler<ChangeMasterVolumeCommand>
{
    private ISettingsModel? _model;
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(ChangeMasterVolumeCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        (_model ??= this.GetModel<ISettingsModel>()!).GetData<AudioSettings>().MasterVolume = input.Volume;
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.Apply<GodotAudioSettings>()
            .ConfigureAwait(false);
        return Unit.Value;
    }
}