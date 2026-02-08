using GFramework.Core.Abstractions.state;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.utils;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
///     暂停游戏命令类，用于执行暂停游戏的操作
/// </summary>
public sealed class PauseGameCommand : AbstractCommand
{
    /// <summary>
    ///     执行暂停游戏命令的具体逻辑
    /// </summary>
    protected override void OnExecute()
    {
        // 设置游戏树的暂停状态为true，实现游戏暂停功能
        GameUtil.GetTree().Paused = true;
        this.GetSystem<IStateMachineSystem>()!.ChangeTo<PausedState>();
    }
}