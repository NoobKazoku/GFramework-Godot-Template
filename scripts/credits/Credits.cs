using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.enums.ui;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.credits;

[ContextAware]
[Log]
public partial class Credits : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IUiRouter _uiRouter = null!;

    private Button BackButton => GetNode<Button>("%BackButton");

    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.Credits);

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
    ///     检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
    /// </summary>
    private async Task CallDeferredInit()
    {
        var env = this.GetEnvironment();
        // 开发环境下检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
        if (GameConstants.Development.Equals(env.Name, StringComparison.Ordinal) && !_uiRouter.IsTop(UiKeyStr))
            await _uiRouter.PushAsync(GetPage()).ConfigureAwait(false);
        // 在此添加延迟初始化逻辑
    }

    /// <summary>
    ///     节点准备就绪时的回调方法
    ///     在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _ = ReadyAsync();
    }

    /// <summary>
    ///     异步等待架构准备完成并获取UI路由器系统
    /// </summary>
    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        _uiRouter = this.GetSystem<IUiRouter>()!;

        // 在此添加就绪逻辑
        SetupEventHandlers();
        // 这个需要延迟调用，因为UiRoot还没有添加到场景树中
        CallDeferred(nameof(CallDeferredInit));
    }

    private void SetupEventHandlers()
    {
        BackButton.Pressed += OnBackButton;
    }

    private void OnBackButton()
    {
        _uiRouter.PopAsync().AsTask().ToCoroutineEnumerator().RunCoroutine();
    }
}