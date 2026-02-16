using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     暂停游戏并打开暂停菜单命令处理器
/// </summary>
public class
    PauseGameWithOpenPauseMenuCommandHandler : AbstractCommandHandler<PauseGameWithOpenPauseMenuCommand, UiHandle>
{
    public override ValueTask<UiHandle> Handle(PauseGameWithOpenPauseMenuCommand command,
        CancellationToken cancellationToken)
    {
        // 发送暂停游戏命令
        this.SendCommand(new PauseGameCommand());

        // 发送打开暂停菜单命令并返回结果
        var handle = this.SendCommand(new OpenPauseMenuCommand(command.Input));
        return ValueTask.FromResult(handle);
    }
}