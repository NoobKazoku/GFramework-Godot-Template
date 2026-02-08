// meta-name: 简单模态UI控制器类模板
// meta-description: 负责管理UI页面场景的生命周期和架构关联
using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.enums.ui;
using GFrameworkGodotTemplate.global;


[ContextAware]
[Log]
public partial class _CLASS_ :_BASE_,IController,IUiPageBehaviorProvider,ISimpleUiPage
{
	/// <summary>
    /// 页面行为实例的私有字段
    /// </summary>
	private IUiPageBehavior? _page;
    
    private IUiRouter _uiRouter = null!;
    
    /// <summary>
    ///  Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey._CLASS_);
    

    /// <summary>
    /// 获取当前UI页面的行为实例。
    /// 如果_page字段尚未初始化，则通过UiPageBehaviorFactory创建一个新的实例并赋值给_page。
    /// 返回类型为IUiPageBehavior，表示UI页面的行为接口。
    /// </summary>
    /// <returns>返回当前UI页面的行为实例。</returns>
    public IUiPageBehavior GetPage()
    {
        // 如果_page为空，则使用UiPageBehaviorFactory创建一个基于_BASE_类型的页面行为实例
        _page ??= UiPageBehaviorFactory.Create<_BASE_>(this, UiKeyStr, UiLayer.Modal, UiLayer.Model);
        return _page;
    }

	
    /// <summary>
    /// 检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
    /// </summary>
    private void CallDeferredInit()
    {
        // 在此添加延迟初始化逻辑
    }
    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _ = ReadyAsync();
    }
    /// <summary>
    /// 异步等待架构准备完成并获取UI路由器系统
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
    /// <summary>
    /// 设置事件处理器
    /// </summary>
    private void SetupEventHandlers()
    {
        
    }
}
