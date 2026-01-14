using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.scenes.tests.ui.main_menu;

[ContextAware]
[Log]
public partial class TestMainMenu : Control, IController,IUiPageProvider
{
    private Button Page1Button => GetNode<Button>("%Page1Button");
    private Button Page2Button => GetNode<Button>("%Page2Button");
    private Button Page3Button => GetNode<Button>("%Page3Button");

    private ControlUiPageBehavior? _page;
    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        var uiRouter = this.GetSystem<IUiRouter>()!;
        Page1Button.Pressed += () =>
        {
            uiRouter.Replace(UiKeys.Page1);
            uiRouter.RegisterOneShot(
                999,
                UITransitionPhases.AfterChange,
                (@event, phase) => string.Equals(@event.ToUiKey, UiKeys.Page1, System.StringComparison.Ordinal),
                @event => {
                    _log.Info("Page1 首次进入教程");
                }
            );
        };
        Page2Button.Pressed += () => { uiRouter.Replace(UiKeys.Page2); };
        Page3Button.Pressed += () => { uiRouter.Replace(UiKeys.Page3); };
    }

    public void OnEnter(IUiPageEnterParam? param)
    {
        _log.Info("测试主菜单 OnEnter");
    }
    public IUiPage GetPage()
    {
        _page ??= new ControlUiPageBehavior(this);
        return _page;
    }
}