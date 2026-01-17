using GFramework.Core.Abstractions.state;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFrameworkGodotTemplate.scripts.command.game.input;
using GFrameworkGodotTemplate.scripts.core.state.impls;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
/// 暂停游戏命令类，用于执行暂停游戏的操作
/// </summary>
/// <param name="input">暂停游戏命令输入参数</param>
public sealed class PauseGameCommand(PauseGameCommandInput input) : AbstractCommand<PauseGameCommandInput>(input)
{
    /// <summary>
    /// 执行暂停游戏命令的具体逻辑
    /// </summary>
    /// <param name="input">暂停游戏命令输入参数，包含需要暂停的节点信息</param>
    protected override void OnExecute(PauseGameCommandInput input)
    {
        // 设置游戏树的暂停状态为true，实现游戏暂停功能
        input.Node.GetTree().Paused = true;
        this.GetSystem<IStateMachineSystem>()!.ChangeTo<PausedState>();
    }
}