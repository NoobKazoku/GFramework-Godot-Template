using GFramework.Core.Abstractions.state;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.utils;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
///     恢复游戏命令类，用于取消游戏暂停状态
/// </summary>
public sealed class ResumeGameCommand : AbstractCommand
{
    /// <summary>
    ///     执行恢复游戏命令的具体逻辑
    /// </summary>
    protected override void OnExecute()
    {
        GameUtil.GetTree().Paused = false;
        this.GetSystem<IStateMachineSystem>()!.ChangeTo<PlayingState>();
    }
}