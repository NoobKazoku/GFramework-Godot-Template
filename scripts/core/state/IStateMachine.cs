using System;

namespace GFrameworkGodotTemplate.scripts.core.state;

/// <summary>
/// 状态机接口，用于管理游戏状态的切换和维护
/// </summary>
public interface IStateMachine
{
    /// <summary>
    /// 当前状态
    /// </summary>
    IState? CurrentState { get; }

    /// <summary>
    /// 切换到指定类型的状态
    /// </summary>
    /// <param name="stateType">目标状态的类型</param>
    public void ChangeState(Type stateType);
    
    /// <summary>
    /// 注册状态到状态机
    /// </summary>
    /// <param name="state">要注册的状态</param>
    void RegisterState(IState state);
    
    /// <summary>
    /// 从状态机中注销指定类型的状态
    /// </summary>
    /// <typeparam name="T">要注销的状态类型</typeparam>
    void UnregisterState<T>() where T : IState;
}

