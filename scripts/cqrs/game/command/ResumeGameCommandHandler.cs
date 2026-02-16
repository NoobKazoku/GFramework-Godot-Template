using GFramework.Core.Abstractions.state;
using GFramework.Core.cqrs.command;
using GFramework.Godot.coroutine;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.utils;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     恢复游戏命令处理器
/// </summary>
public class ResumeGameCommandHandler : AbstractCommandHandler<ResumeGameCommand>
{
    private IStateMachineSystem? _stateMachineSystem;

    public override ValueTask<Unit> Handle(ResumeGameCommand command, CancellationToken cancellationToken)
    {
        GameUtil.GetTree().Paused = false;
        (_stateMachineSystem ??= this.GetSystem<IStateMachineSystem>())!
            .ChangeToAsync<PlayingState>()
            .ToCoroutineEnumerator()
            .RunCoroutine();
        return ValueTask.FromResult(Unit.Value);
    }
}