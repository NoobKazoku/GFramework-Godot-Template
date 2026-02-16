using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command;

/// <summary>
///     关闭暂停菜单命令处理器
/// </summary>
public class ClosePauseMenuCommandHandler : AbstractCommandHandler<ClosePauseMenuCommand>
{
    private IUiRouter? _uiRouter;

    public override ValueTask<Unit> Handle(ClosePauseMenuCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        (_uiRouter ??= this.GetSystem<IUiRouter>())!.Hide(input.Handle, UiLayer.Modal);
        return ValueTask.FromResult(Unit.Value);
    }
}