using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.scenes.tests.ui;

[ContextAware]
[Log]
public partial class Page2 : Control,IController,IUiPageBehaviorProvider,ISimpleUiPage
{
	private IUiPageBehavior? _page;
	public IUiPageBehavior GetPage()
	{
		_page ??= new CanvasItemUiPageBehavior<Control>(this);
		return _page;
	}
	private Button MainMenuButton=> GetNode<Button>("%MainMenuButton");
	private Button Page1Button => GetNode<Button>("%Page1Button");
	private Button Page3Button => GetNode<Button>("%Page3Button");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		var uiRouter = this.GetSystem<IUiRouter>()!;
		MainMenuButton.Pressed += () => { uiRouter.Replace(UiKeys.MainMenu); };
		Page1Button.Pressed += () => { uiRouter.Replace(UiKeys.Page1); };
		Page3Button.Pressed += () => { uiRouter.Replace(UiKeys.Page3); };
	}

	public void OnEnter(IUiPageEnterParam? param)
	{
		_log.Info("Page2 OnEnter");
	}
	
}