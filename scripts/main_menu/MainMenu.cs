using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.state;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.cqrs.game.command;
using GFrameworkGodotTemplate.scripts.cqrs.menu.command;
using GFrameworkGodotTemplate.scripts.credits;
using GFrameworkGodotTemplate.scripts.enums.ui;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.main_menu;

/// <summary>
///     主菜单控制器类，继承自Control并实现IController、IUiPageBehaviorProvider和ISimpleUiPage接口
///     负责处理主菜单界面的逻辑和生命周期管理
/// </summary>
[ContextAware]
[Log]
public partial class MainMenu : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IStateMachineSystem _stateMachineSystem = null!;

    private IUiRouter _uiRouter = null!;
    private Button NewGameButton => GetNode<Button>("%NewGameButton");
    private Button ContinueGameButton => GetNode<Button>("%ContinueGameButton");
    private Button OptionsMenuButton => GetNode<Button>("%OptionsMenuButton");
    private Button CreditsButton => GetNode<Button>("%CreditsButton");
    private Button ExitButton => GetNode<Button>("%ExitButton");

    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.MainMenu);

    /// <summary>
    ///     获取页面行为实例，如果不存在则创建新的CanvasItemUiPageBehavior实例
    /// </summary>
    /// <returns>返回IUiPageBehavior类型的页面行为实例</returns>
    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<Control>(this, UiKeyStr, UiLayer.Page);
        return _page;
    }

    /// <summary>
    ///     节点准备就绪时的回调方法
    ///     在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _ = ReadyAsync();
    }

    private async Task ReadyAsync()
    {
        // 等待游戏架构初始化完成
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        // 获取UI路由器实例
        _uiRouter = this.GetSystem<IUiRouter>()!;
        _stateMachineSystem = this.GetSystem<IStateMachineSystem>()!;
        SetupEventHandlers();
    }

    private void SetupEventHandlers()
    {
        // 绑定退出游戏按钮点击事件
        ExitButton.Pressed += () => this.RunCommandCoroutine(new ExitGameCommand());
        // 绑定制作组按钮点击事件
        CreditsButton.Pressed += () =>
        {
            _uiRouter.PushAsync(Credits.UiKeyStr).AsTask().ToCoroutineEnumerator().RunCoroutine();
        };
        OptionsMenuButton.Pressed += () => { this.RunCommandCoroutine(new OpenOptionsMenuCommand()); };
        NewGameButton.Pressed += () =>
        {
            _stateMachineSystem.ChangeToAsync<PlayingState>().ToCoroutineEnumerator().RunCoroutine();
        };
    }
}