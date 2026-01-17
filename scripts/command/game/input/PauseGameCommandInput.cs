
using GFramework.Core.Abstractions.command;
using Godot;

namespace GFrameworkGodotTemplate.scripts.command.game.input;

/// <summary>
/// 暂停游戏命令输入类，定义执行暂停游戏命令所需的输入参数
/// </summary>
public sealed class PauseGameCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置需要暂停的游戏节点
    /// </summary>
    public required Node Node { get; init; }
}