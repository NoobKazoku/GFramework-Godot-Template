using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.scenes.tests.ui;

[ContextAware]
[Log]
public partial class Page1 : Control,IController,IUiPageProvider
{
	private ControlUiPageBehavior? _page;
	private Button MainMenuButton=> GetNode<Button>("%MainMenuButton");
	private Button Page2Button => GetNode<Button>("%Page2Button");
	private Button Page3Button => GetNode<Button>("%Page3Button");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		var uiRouter = this.GetSystem<IUiRouter>()!;
		MainMenuButton.Pressed += () => { uiRouter.Replace(UiKeys.MainMenu); };
		Page2Button.Pressed += () => { uiRouter.Replace(UiKeys.Page2); };
		Page3Button.Pressed += () => { uiRouter.Replace(UiKeys.Page3); };
	}

	public void OnEnter(IUiPageEnterParam? param)
	{
		_log.Info("Page1 OnEnter");
	}
	public IUiPage GetPage()
	{
		_page ??= new ControlUiPageBehavior(this);
		return _page;
	}
}
