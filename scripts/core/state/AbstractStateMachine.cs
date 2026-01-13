using System;
using System.Collections.Generic;
using GFramework.Core.extensions;
using GFramework.Core.system;

namespace GFrameworkGodotTemplate.scripts.core.state;

/// <summary>
/// 状态机抽象基类，实现了IStateMachine接口的核心逻辑
/// </summary>
public abstract class AbstractStateMachine : AbstractSystem, IStateMachine
{
    /// <summary>
    /// 已注册的状态字典
    /// </summary>
    protected readonly IDictionary<string, IState> States = new Dictionary<string, IState>(StringComparer.Ordinal);
    
    /// <summary>
    /// 当前状态
    /// </summary>
    public IState? CurrentState { get; private set; }
    
    /// <summary>
    /// 切换到新状态
    /// </summary>
    /// <param name="newState">要切换到的新状态</param>
    public void ChangeState(IState newState)
    {
        if (CurrentState == newState)
            return;
        
        if (CurrentState != null && !CurrentState.CanTransitionTo(newState))
            return;
        
        var oldState = CurrentState;
        
        if (CurrentState != null)
            CurrentState.OnExit(newState);
        
        CurrentState = newState;
        CurrentState.OnEnter(oldState);
        this.SendEvent(new StateChangedEvent()
        {
            OldState = oldState,
            NewState = CurrentState,
        });
    }
    
    /// <summary>
    /// 注册状态到状态机
    /// </summary>
    /// <param name="state">要注册的状态</param>
    public void RegisterState(IState state)
    {
        var stateKey = GetStateKey(state);
        States.TryAdd(stateKey, state);
    }
    
    /// <summary>
    /// 从状态机注销状态
    /// </summary>
    /// <param name="state">要注销的状态</param>
    public void UnregisterState(IState state)
    {
        var stateKey = GetStateKey(state);
        States.Remove(stateKey);
    }

    /// <summary>
    /// 获取状态的唯一标识键，子类可重写以自定义键生成逻辑
    /// </summary>
    /// <param name="state">要获取键的状态</param>
    /// <returns>状态键</returns>
    protected virtual string GetStateKey(IState state)
    {
        return state.GetType().Name;
    }
    
    /// <summary>
    /// 初始化时为所有状态设置上下文
    /// 遍历所有状态并调用SetContext方法注入上下文对象
    /// </summary>
    protected override void OnInit()
    {
        foreach (var state in States.Values)
        {
            state.SetContext(Context!);
        }
    }

}
