namespace GFrameworkGodotTemplate.scripts.core.state;

/// <summary>
/// 游戏状态机实现类，用于管理游戏流程状态
/// </summary>
public class GameStateMachine : StateMachineBase
{
    public bool IsIn<T>() where T : IState
        => CurrentState is T;

    public T? Get<T>() where T : class, IState
        => CurrentState as T;
}
