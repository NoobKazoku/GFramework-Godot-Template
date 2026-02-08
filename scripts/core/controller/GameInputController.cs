using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFrameworkGodotTemplate.scripts.enums;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.controller;

/// <summary>
///     游戏输入控制器抽象基类，继承自Node并实现IController接口
///     负责处理游戏中的未处理输入事件
/// </summary>
[Log]
public abstract partial class GameInputController : Node, IController
{
    /// <summary>
    ///     获取当前场景树的引用
    /// </summary>
    protected SceneTree Tree => GetTree();

    /// <summary>
    ///     处理输入事件的主入口方法
    ///     该方法会分发全局输入事件以及根据游戏暂停状态分发 gameplay 或 paused 阶段的输入事件
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void _Input(InputEvent @event)
    {
        var pausedAtFrameStart = Tree.Paused;

        Dispatch(InputPhase.Global, @event);

        Dispatch(pausedAtFrameStart ? InputPhase.Paused : InputPhase.Gameplay, @event);
    }

    /// <summary>
    ///     分发输入事件到具体的处理逻辑
    ///     该方法会检查当前阶段是否被接受以及是否被阻塞，若满足条件则调用 Handle 方法进行处理
    /// </summary>
    /// <param name="phase">输入阶段（如 Global、Gameplay、Paused）</param>
    /// <param name="event">输入事件对象</param>
    private void Dispatch(InputPhase phase, InputEvent @event)
    {
        // 检查当前阶段是否被接受
        if (!AcceptPhase(phase))
            return;

        // 检查当前是否被阻塞
        if (IsBlocked())
            return;

        // 调用具体处理逻辑
        Handle(phase, @event);
    }

    /// <summary>
    ///     判断当前输入阶段是否被接受
    ///     默认返回 false，子类可重写以定义特定阶段的接受规则
    /// </summary>
    /// <param name="phase">输入阶段</param>
    /// <returns>如果阶段被接受则返回 true，否则返回 false</returns>
    protected virtual bool AcceptPhase(InputPhase phase)
    {
        return false;
    }

    /// <summary>
    ///     抽象方法，用于处理具体的输入事件
    ///     子类必须实现此方法以定义不同阶段下的输入处理逻辑
    /// </summary>
    /// <param name="phase">输入阶段</param>
    /// <param name="event">输入事件对象</param>
    protected abstract void Handle(InputPhase phase, InputEvent @event);

    /// <summary>
    ///     判断当前输入是否被阻塞
    ///     默认返回 false，子类可重写以定义阻塞条件
    /// </summary>
    /// <returns>如果输入被阻塞则返回 true，否则返回 false</returns>
    protected virtual bool IsBlocked()
    {
        return false;
    }
}