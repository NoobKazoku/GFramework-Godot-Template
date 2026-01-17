using GFramework.Core.command;
using GFrameworkGodotTemplate.scripts.command.game.input;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
/// 退出游戏命令类，用于处理游戏退出逻辑
/// </summary>
/// <param name="input">退出游戏命令输入参数</param>
public sealed class QuitGameCommand(QuitGameCommandInput input) : AbstractCommand<QuitGameCommandInput>(input)
{
    /// <summary>
    /// 执行退出游戏命令的具体逻辑
    /// </summary>
    /// <param name="input">退出游戏命令输入参数，包含执行退出操作所需的节点信息</param>
    protected override void OnExecute(QuitGameCommandInput input)
    {
        input.Node.GetTree().Quit();
    }
}
