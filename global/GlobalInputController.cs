using GFramework.Core.Abstractions.state;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.command.game;
using GFrameworkGodotTemplate.scripts.command.game.input;
using GFrameworkGodotTemplate.scripts.core.controller;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.events.menu;
using Godot;

namespace GFrameworkGodotTemplate.global;

[ContextAware]
[Log]
public partial class GlobalInputController :GameInputController
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
		if (current is not PlayingState)
		{
			return;
		}
		// 根据当前游戏暂停状态决定执行暂停或恢复命令
		if (current is PausedState)
		{
			_log.Debug("恢复游戏");
			// 当前是暂停 → 恢复游戏 → 关闭暂停菜单
			this.SendEvent<ClosePauseMenuEvent>();
			this.SendCommand(new ResumeGameCommand(new ResumeGameCommandInput{Node = this}));
		}
		else
		{
			_log.Debug("暂停游戏");
			// 当前是运行 → 暂停游戏 → 打开暂停菜单
			this.SendCommand(new PauseGameCommand(new PauseGameCommandInput{Node = this}));
			this.SendEvent<OpenPauseMenuEvent>();
		}

	}
}