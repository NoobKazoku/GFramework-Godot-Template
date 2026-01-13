using System;
using GFramework.Core.Abstractions.rule;

namespace GFrameworkGodotTemplate.scripts.core.state;

/// <summary>
/// 状态接口，定义了游戏状态的基本行为
/// </summary>
public interface IState : IContextAware
{
    /// <summary>
    /// 状态的标识符
    /// </summary>
    public Type Key() => GetType();

    /// <summary>
    /// 进入状态时调用
    /// </summary>
    /// <param name="fromState">上一个状态，如果是首次进入则为null</param>
    void OnEnter(IState? fromState);

    /// <summary>
    /// 退出状态时调用
    /// </summary>
    /// <param name="toState">下一个状态，如果是最后退出则为null</param>
    void OnExit(IState? toState);

    /// <summary>
    /// 判断是否可以转换到目标状态
    /// </summary>
    /// <param name="targetState">目标状态</param>
    /// <returns>是否允许转换</returns>
    bool CanTransitionTo(IState targetState);
}