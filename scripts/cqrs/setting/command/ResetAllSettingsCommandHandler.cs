using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.setting;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.command;

/// <summary>
///     重置所有设置命令处理器
/// </summary>
public class ResetAllSettingsCommandHandler : AbstractCommandHandler<ResetAllSettingsCommand>
{
    private ISettingsSystem? _settingsSystem;

    public override async ValueTask<Unit> Handle(ResetAllSettingsCommand command, CancellationToken cancellationToken)
    {
        await (_settingsSystem ??= this.GetSystem<ISettingsSystem>())!.ResetAll().ConfigureAwait(false);
        return Unit.Value;
    }
}