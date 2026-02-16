using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.command;

/// <summary>
///     更改语言命令处理器
/// </summary>
public class ChangeLanguageCommandHandler : AbstractCommandHandler<ChangeLanguageCommand>
{
    private ISettingsModel? _model;
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(ChangeLanguageCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        var settings = (_model ??= this.GetModel<ISettingsModel>()!).GetData<LocalizationSettings>();
        settings.Language = input.Language;
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.Apply<GodotLocalizationSettings>()
            .ConfigureAwait(false);
        return Unit.Value;
    }
}