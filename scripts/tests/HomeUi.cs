using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.coroutine;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.scene;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using GFramework.Godot.scene;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.enums.scene;
using GFrameworkGodotTemplate.scripts.enums.ui;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.tests;

[ContextAware]
[Log]
public partial class HomeUi : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IGodotSceneRegistry _sceneRegistry = null!;

    private ISceneRouter _sceneRouter = null!;

    private Button Scene1Button => GetNode<Button>("%Scene1Button");

    private Button Scene2Button => GetNode<Button>("%Scene2Button");

    private Button HomeUiButton => GetNode<Button>("%HomeButton");

    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.HomeUi);

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
    private void CallDeferredInit()
    {
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
        Hide();
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        _sceneRouter = this.GetSystem<ISceneRouter>()!;
        _sceneRegistry = this.GetUtility<IGodotSceneRegistry>()!;

        // 在此添加就绪逻辑
        SetupEventHandlers();
        // 这个需要延迟调用，因为UiRoot还没有添加到场景树中
        CallDeferred(nameof(CallDeferredInit));
        Show();
    }

    /// <summary>
    ///     设置事件处理器
    /// </summary>
    private void SetupEventHandlers()
    {
        var transitionManager = SceneTransitionManager.Instance!;

        IEnumerator<IYieldInstruction> ReplaceScene(string key)
        {
            _sceneRouter.Replace(key);
            yield return null;
        }

        // 场景预加载器 - 根据场景key加载对应的场景
        Node PreloadScene(string sceneKey)
        {
            var packedScene = _sceneRegistry.Get(sceneKey);
            return packedScene.Instantiate();
        }

        Scene1Button.Pressed += () =>
        {
            var sceneKey = nameof(SceneKey.Scene1);
            transitionManager
                .PlayTransitionCoroutine(
                    ReplaceScene(sceneKey),
                    () => PreloadScene(sceneKey)
                )
                .RunCoroutine();
        };

        Scene2Button.Pressed += () =>
        {
            var sceneKey = nameof(SceneKey.Scene2);
            transitionManager
                .PlayTransitionCoroutine(
                    ReplaceScene(sceneKey),
                    () => PreloadScene(sceneKey)
                )
                .RunCoroutine();
        };

        HomeUiButton.Pressed += () =>
        {
            var sceneKey = nameof(SceneKey.Home);
            transitionManager
                .PlayTransitionCoroutine(
                    ReplaceScene(sceneKey),
                    () => PreloadScene(sceneKey)
                )
                .RunCoroutine();
        };
    }
}