using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.scenes.tests.ui;

[ContextAware]
[Log]
public partial class Page3 : Control,IController
{
	private ControlUiPageBehavior _page = null!;
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		_page = new ControlUiPageBehavior(this);
	}

	public void OnEnter(IUiPageEnterParam? param)
	{
		_log.Info("Page3 OnEnter");
	}
	public IUiPage AsPage() => _page;
}