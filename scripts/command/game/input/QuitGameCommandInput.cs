
using GFramework.Core.Abstractions.command;
using Godot;

namespace GFrameworkGodotTemplate.scripts.command.game.input;

/// <summary>
/// 退出游戏命令输入类，封装执行退出游戏命令所需的数据
/// </summary>
public sealed class QuitGameCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置用于执行退出操作的节点对象
    /// </summary>
    public required Node Node { get; init; }
}
