using GFramework.Core.Abstractions.state;
using GFramework.Game.Abstractions.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.controller;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.cqrs.game.command;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command.input;
using GFrameworkGodotTemplate.scripts.enums;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
///     全局输入控制器类，继承自 GameInputController。
///     负责处理游戏中的全局输入事件，包括暂停和恢复游戏的功能。
/// </summary>
[ContextAware]
[Log]
public partial class GlobalInputController : GameInputController
{
    private UiHandle? _pauseMenuUiHandle;

    /// <summary>
    ///     状态机系统实例，用于管理游戏状态。
    /// </summary>
    private IStateMachineSystem _stateMachineSystem = null!;

    /// <summary>
    ///     初始化方法，在节点准备就绪时调用。
    ///     获取并初始化状态机系统实例。
    /// </summary>
    public override void _Ready()
    {
        _stateMachineSystem = this.GetSystem<IStateMachineSystem>()!;
    }

    protected override bool AcceptPhase(InputPhase phase)
    {
        return phase is InputPhase.Global or InputPhase.Paused;
    }

    protected override void Handle(InputPhase phase, InputEvent @event)
    {
        // 检查是否按下了取消操作（通常是 ESC 键）
        if (!@event.IsActionPressed("ui_cancel"))
            return;

        // 根据当前状态执行相应操作
        if (_stateMachineSystem.Current is not PlayingState) return;
        _log.Debug("暂停游戏");
        _pauseMenuUiHandle = this.SendCommand(new PauseGameWithOpenPauseMenuCommand(new OpenPauseMenuCommandInput
            { Handle = _pauseMenuUiHandle }));
        GetViewport().SetInputAsHandled();
    }
}