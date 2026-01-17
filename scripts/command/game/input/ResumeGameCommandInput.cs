using GFramework.Core.Abstractions.command;
using Godot;

namespace GFrameworkGodotTemplate.scripts.command.game.input;

/// <summary>
/// 恢复游戏命令输入类，封装执行恢复游戏操作所需的数据
/// </summary>
public sealed class ResumeGameCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置用于执行退出操作的节点对象
    /// </summary>
    public required Node Node { get; init; }
}