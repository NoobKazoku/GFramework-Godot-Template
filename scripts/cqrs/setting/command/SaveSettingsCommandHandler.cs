using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.command;

/// <summary>
///     保存游戏设置命令处理器
/// </summary>
public class SaveSettingsCommandHandler : AbstractCommandHandler<SaveSettingsCommand>
{
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(SaveSettingsCommand command, CancellationToken cancellationToken)
    {
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.SaveAll().ConfigureAwait(false);
        return Unit.Value;
    }
}