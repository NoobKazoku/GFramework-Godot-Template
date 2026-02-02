using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.enums.ui;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.main_menu;

/// <summary>
/// 主菜单控制器类，继承自Control并实现IController、IUiPageBehaviorProvider和ISimpleUiPage接口
/// 负责处理主菜单界面的逻辑和生命周期管理
/// </summary>
[ContextAware]
[Log]
public partial class MainMenu : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    /// 页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IUiRouter _uiRouter = null!;

    /// <summary>
    ///  Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.MainMenu);

    /// <summary>
    /// 获取页面行为实例，如果不存在则创建新的CanvasItemUiPageBehavior实例
    /// </summary>
    /// <returns>返回IUiPageBehavior类型的页面行为实例</returns>
    public IUiPageBehavior GetPage()
    {
        _page ??= new CanvasItemUiPageBehavior<Control>(this, UiKeyStr);
        return _page;
    }

    /// <summary>
    /// 检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
    /// </summary>
    private void CallDeferredInit()
    {
        var env = this.GetEnvironment();
        // 在开发环境中且当前页面不在路由栈顶时，将页面推入路由栈
        if (GameConstants.Development.Equals(env.Name, StringComparison.Ordinal) && !_uiRouter.IsTop(UiKeyStr))
        {
            _uiRouter.Push(GetPage());
        }
    }

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
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
        // 延迟调用初始化方法
        CallDeferred(nameof(CallDeferredInit));
    }
}