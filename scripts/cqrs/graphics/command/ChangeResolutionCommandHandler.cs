using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     更改分辨率命令处理器
/// </summary>
public class ChangeResolutionCommandHandler : AbstractCommandHandler<ChangeResolutionCommand>
{
    private ISettingsModel? _model;
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(ChangeResolutionCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        var settings = (_model ??= this.GetModel<ISettingsModel>()!).GetData<GraphicsSettings>();
        settings.ResolutionWidth = input.Width;
        settings.ResolutionHeight = input.Height;
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.Apply<GodotGraphicsSettings>()
            .ConfigureAwait(false);
        return Unit.Value;
    }
}