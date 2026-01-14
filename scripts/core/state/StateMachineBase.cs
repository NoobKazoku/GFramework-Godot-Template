using System;
using System.Collections.Generic;
using GFramework.Core.extensions;
using GFramework.Core.system;

namespace GFrameworkGodotTemplate.scripts.core.state;

/// <summary>
/// 状态机基类，实现了IStateMachine接口的核心逻辑
/// 提供状态管理、状态切换、状态注册等功能
/// </summary>
public class StateMachineBase : AbstractSystem, IStateMachine
{
    /// <summary>
    /// 已注册的状态字典，用于存储类型到状态实例的映射关系
    /// </summary>
    protected readonly IDictionary<Type, IState> States = new Dictionary<Type, IState>();

    /// <summary>
    /// 当前状态，表示状态机当前处于的状态
    /// </summary>
    public IState? CurrentState { get; private set; }

    /// <summary>
    /// 切换到新状态
    /// 执行状态切换的完整流程：验证切换条件、执行退出操作、更新当前状态、执行进入操作、发送状态变更事件
    /// </summary>
    /// <param name="newState">要切换到的新状态实例</param>
    public void ChangeState(IState newState)
    {
        if (CurrentState == newState)
            return;

        if (CurrentState != null && !CurrentState.CanTransitionTo(newState))
            return;

        var oldState = CurrentState;

        CurrentState?.OnExit(newState);

        CurrentState = newState;
        CurrentState.OnEnter(oldState);
        this.SendEvent(new StateChangedEvent()
        {
            OldState = oldState,
            NewState = CurrentState,
        });
    }
    /// <summary>
    /// 切换到指定泛型类型的状态
    /// 通过泛型类型参数快速切换到对应的状态实例
    /// </summary>
    /// <typeparam name="T">目标状态的类型，必须实现IState接口</typeparam>
    public void ChangeState<T>() where T : IState
    {
        ChangeState(typeof(T));
    }
    /// <summary>
    /// 根据状态类型切换到对应状态
    /// 通过类型查找已注册的状态实例并执行切换操作
    /// </summary>
    /// <param name="stateType">要切换到的状态类型</param>
    /// <exception cref="InvalidOperationException">当指定类型的状态未注册时抛出异常</exception>
    public void ChangeState(Type stateType)
    {
        if (!States.TryGetValue(stateType, out var newState))
            throw new InvalidOperationException($"State {stateType.Name} not registered.");

        ChangeStateInternal(newState);
    }
    
    /// <summary>
    /// 内部状态切换方法，执行实际的状态切换逻辑
    /// 包含状态验证、退出旧状态、进入新状态、发送事件等步骤
    /// </summary>
    /// <param name="newState">要切换到的新状态实例</param>
    private void ChangeStateInternal(IState newState)
    {
        if (CurrentState == newState)
            return;

        if (CurrentState != null && !CurrentState.CanTransitionTo(newState))
            return;

        var oldState = CurrentState;

        oldState?.OnExit(newState);

        CurrentState = newState;
        CurrentState.OnEnter(oldState);

        this.SendEvent(new StateChangedEvent
        {
            OldState = oldState,
            NewState = CurrentState
        });
    }

    /// <summary>
    /// 注册状态到状态机
    /// 将状态实例添加到内部字典中，并设置状态的上下文环境
    /// </summary>
    /// <param name="state">要注册的状态实例</param>
    public void RegisterState(IState state)
    {
        var type = state.GetType();
        States.TryAdd(type, state);
    }

    /// <summary>
    /// 从状态机中注销指定类型的状态
    /// 如果该状态正在运行，则先安全退出该状态，然后从注册表中移除
    /// </summary>
    /// <typeparam name="T">要注销的状态类型，必须实现IState接口</typeparam>
    public void UnregisterState<T>() where T : IState
    {
        var type = typeof(T);

        if (!States.TryGetValue(type, out var state))
            return;

        // 如果正在运行，先安全退出
        if (CurrentState == state)
        {
            CurrentState.OnExit(toState: null);
            CurrentState = null;
        }

        States.Remove(type);
    }
    
    
    /// <summary>
    /// 获取状态的唯一标识键，子类可重写以自定义键生成逻辑
    /// 默认使用状态类型的名称作为键值
    /// </summary>
    /// <param name="state">要获取键的状态实例</param>
    /// <returns>状态的字符串键值</returns>
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
