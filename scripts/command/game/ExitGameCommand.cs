using GFramework.Core.command;
using GFrameworkGodotTemplate.scripts.core.utils;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
///     退出游戏命令类，用于处理游戏退出逻辑
/// </summary>
public sealed class ExitGameCommand : AbstractCommand
{
    /// <summary>
    ///     执行退出游戏命令的具体逻辑
    /// </summary>
    protected override void OnExecute()
    {
        GameUtil.GetTree().Quit();
    }
}