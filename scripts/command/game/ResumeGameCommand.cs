using GFramework.Core.Abstractions.state;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Game.state;
using GFrameworkGodotTemplate.scripts.command.game.input;
using GFrameworkGodotTemplate.scripts.core.state.impls;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
/// 恢复游戏命令类，用于取消游戏暂停状态
/// </summary>
/// <param name="input">恢复游戏命令输入参数</param>
public sealed class ResumeGameCommand(ResumeGameCommandInput input) : AbstractCommand<ResumeGameCommandInput>(input)
{
    /// <summary>
    /// 执行恢复游戏命令的具体逻辑
    /// </summary>
    /// <param name="input">恢复游戏命令输入参数，包含执行操作所需的节点信息</param>
    protected override void OnExecute(ResumeGameCommandInput input)
    {
        input.Node.GetTree().Paused = false;
        this.GetSystem<IStateMachineSystem>()!.ChangeTo<PlayingState>();
    }
}


