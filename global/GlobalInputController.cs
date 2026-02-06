using GFramework.Core.Abstractions.state;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.command.game;
using GFrameworkGodotTemplate.scripts.core.controller;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using Godot;

namespace GFrameworkGodotTemplate.global;

[ContextAware]
[Log]
public partial class GlobalInputController : GameInputController
{
    private IStateMachineSystem _stateMachineSystem = null!;

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _stateMachineSystem = this.GetSystem<IStateMachineSystem>()!;
    }

    /// <summary>
    /// 处理输入事件，检测暂停/恢复游戏的按键操作
    /// </summary>
    /// <param name="event">输入事件对象</param>
    protected override void HandleInput(InputEvent @event)
    {
        // 检查是否按下了取消键（通常是ESC键）
        if (!@event.IsActionPressed("ui_cancel"))
        {
            return;
        }

        var current = _stateMachineSystem.Current;

        switch (current)
        {
            case PausedState:
                _log.Debug("恢复游戏");
                this.SendCommand(new ResumeGameWithClosePauseMenuCommand());
                break;
            case PlayingState:
                _log.Debug("暂停游戏");
                this.SendCommand(new PauseGameWithOpenPauseMenuCommand());
                break;
        }
    }
}