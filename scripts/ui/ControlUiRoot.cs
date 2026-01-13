using System;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.scripts.ui;

/// <summary>
/// UI根控制器，继承自Godot Control并实现IUiRoot接口
/// 用于管理UI页面的添加和组织
/// </summary>
[Log]
[ContextAware]
public partial class ControlUiRoot : Control, IUiRoot
{
	public static ControlUiRoot Instance { get; private set; } = null!;
	public override void _Ready()
	{
		Instance = this;
	}

	/// <summary>
	/// 向UI根节点添加UI页面
	/// </summary>
	/// <param name="child">要添加的UI页面，必须实现IUiPage接口</param>
	/// <exception cref="InvalidOperationException">当传入的child不是Godot Node类型时抛出</exception>
	public void AddUiPage(IUiPage child)
	{
		if (child is Node node)
			AddChild(node);
		else
			throw new InvalidOperationException("UIPage must be a Godot Node");
	}

	/// <summary>
	/// 从UI根节点移除UI页面
	/// </summary>
	/// <param name="child">要移除的UI页面，必须实现IUiPage接口</param>
	public void RemoveUiPage(IUiPage child)
	{
		if (child is Node node)
			RemoveChild(node);
	}
}
