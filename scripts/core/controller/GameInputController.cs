using GFramework.Core.Abstractions.controller;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.controller;

/// <summary>
/// 游戏输入控制器抽象基类，继承自Node并实现IController接口
/// 负责处理游戏中的未处理输入事件
/// </summary>
public abstract partial class GameInputController 
    : Node, IController
{
    /// <summary>
    /// 获取场景树实例的属性
    /// </summary>
    protected SceneTree Tree => GetTree();

    /// <summary>
    /// 处理未处理的输入事件
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void _UnhandledInput(InputEvent @event)
    {
        // 检查控制器是否被阻止，如果被阻止则直接返回
        if (IsBlocked()) return;

        // 处理具体的输入事件
        HandleInput(@event);
    }

    /// <summary>
    /// 抽象方法，用于处理具体的输入事件
    /// 子类必须实现此方法来定义输入处理逻辑
    /// </summary>
    /// <param name="event">输入事件对象</param>
    protected abstract void HandleInput(InputEvent @event);

    /// <summary>
    /// 虚拟方法，用于检查控制器是否被阻止
    /// 子类可以重写此方法来自定义阻止逻辑
    /// </summary>
    /// <returns>如果控制器被阻止则返回true，否则返回false</returns>
    protected virtual bool IsBlocked()
    {
        return false;
    }
}
