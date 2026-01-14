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
public partial class Page3 : Control,IController,IUiPageProvider
{
	private ControlUiPageBehavior? _page;
	public IUiPage GetPage()
	{
		_page ??= new ControlUiPageBehavior(this);
		return _page;
	}

	private Button MainMenuButton=> GetNode<Button>("%MainMenuButton");
	private Button Page1Button => GetNode<Button>("%Page1Button");
	private Button Page2Button => GetNode<Button>("%Page2Button");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		var uiRouter = this.GetSystem<IUiRouter>()!;
		MainMenuButton.Pressed += () => { uiRouter.Replace(UiKeys.MainMenu); };
		Page1Button.Pressed += () => { uiRouter.Replace(UiKeys.Page1); };
		Page2Button.Pressed += () => { uiRouter.Replace(UiKeys.Page2); };
	}
	public void OnEnter(IUiPageEnterParam? param)
	{
		_log.Info("Page3 OnEnter");
	}
}