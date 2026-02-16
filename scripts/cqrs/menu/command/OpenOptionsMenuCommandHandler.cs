using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.options_menu;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.menu.command;

/// <summary>
///     打开选项菜单命令处理器
/// </summary>
public class OpenOptionsMenuCommandHandler : AbstractCommandHandler<OpenOptionsMenuCommand>
{
    private IUiRouter? _uiRouter;

    public override ValueTask<Unit> Handle(OpenOptionsMenuCommand command, CancellationToken cancellationToken)
    {
        (_uiRouter ??= this.GetSystem<IUiRouter>())!.Show(OptionsMenu.UiKeyStr, UiLayer.Modal);
        return ValueTask.FromResult(Unit.Value);
    }
}