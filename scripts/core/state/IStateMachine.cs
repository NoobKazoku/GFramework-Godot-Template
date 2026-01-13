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
    /// 切换到新状态
    /// </summary>
    /// <param name="newState">要切换到的新状态</param>
    void ChangeState(IState newState);
    
    /// <summary>
    /// 注册状态到状态机
    /// </summary>
    /// <param name="state">要注册的状态</param>
    void RegisterState(IState state);
    
    /// <summary>
    /// 从状态机注销状态
    /// </summary>
    /// <param name="state">要注销的状态</param>
    void UnregisterState(IState state);
}
