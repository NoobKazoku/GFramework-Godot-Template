using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     恢复游戏并关闭暂停菜单命令处理器
/// </summary>
public class ResumeGameWithClosePauseMenuCommandHandler : AbstractCommandHandler<ResumeGameWithClosePauseMenuCommand>
{
    public override ValueTask<Unit> Handle(ResumeGameWithClosePauseMenuCommand command,
        CancellationToken cancellationToken)
    {
        // 发送关闭暂停菜单的命令
        this.SendCommand(new ClosePauseMenuCommand(command.Input));

        // 发送恢复游戏的命令
        this.SendCommand(new ResumeGameCommand());

        return ValueTask.FromResult(Unit.Value);
    }
}