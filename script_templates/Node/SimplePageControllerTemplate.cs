// meta-name: 简单UI页面控制器类模板
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
    private static string UiKeyStr => nameof(UiKey._CLASS_);
    
    /// <summary>
    /// 获取页面行为实例，如果不存在则创建新的CanvasItemUiPageBehavior实例
    /// </summary>
    /// <returns>返回IUiPageBehavior类型的页面行为实例</returns>
    public IUiPageBehavior GetPage()
    {
        _page ??= new CanvasItemUiPageBehavior<_BASE_>(this,UiKeyStr);
        return _page;
    }
	
    /// <summary>
    /// 检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
    /// </summary>
    private void CallDeferredInit()
    {
        var env = this.GetEnvironment();
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
        _uiRouter = this.GetSystem<IUiRouter>()!;
        CallDeferred(nameof(CallDeferredInit));
    }
    /// <summary>
    /// 页面进入时调用的方法
    /// </summary>
    /// <param name="param">页面进入参数，可能为空</param>
    public void OnEnter(IUiPageEnterParam? param)
    {
       
    }
}