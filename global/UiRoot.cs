using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.constants;
using GFrameworkGodotTemplate.scripts.cqrs.global.events;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
///     UI画布层根节点，用于管理UI页面的添加和组织
///     继承自CanvasLayer并实现IUiRoot接口
/// </summary>
[Log]
[ContextAware]
public partial class UiRoot : CanvasLayer, IUiRoot
{
    private readonly Dictionary<UiLayer, Control> _containers = new();
    private readonly List<IUiPageBehavior> _pages = new();

    /// <summary>
    ///     向UI根节点添加UI页面
    /// </summary>
    /// <param name="child">要添加的UI页面行为对象</param>
    public void AddUiPage(IUiPageBehavior child)
    {
        AddUiPage(child, UiLayer.Page);
    }

    /// <summary>
    ///     向指定UI层添加UI页面
    /// </summary>
    /// <param name="child">要添加的UI页面行为对象</param>
    /// <param name="layer">目标UI层</param>
    /// <param name="orderInLayer">层内排序序号</param>
    public void AddUiPage(IUiPageBehavior child, UiLayer layer, int orderInLayer = 0)
    {
        if (child.View is not CanvasItem item)
            throw new InvalidOperationException("UIPage View must be a Godot Node");

        if (!_containers.TryGetValue(layer, out var container))
            throw new InvalidOperationException($"UiLayer not found: {layer}");

        if (item.GetParent() == null)
            container.AddChild(item);
        else if (item.GetParent() != container) item.Reparent(container);

        // 设置Z轴索引以控制渲染顺序
        item.ZIndex = (int)layer * 100 + orderInLayer;
        item.ZAsRelative = false;

        if (!_pages.Contains(child))
            _pages.Add(child);

        _log.Debug($"Add UI [{child.Key}] Layer={layer} Order={orderInLayer}");
    }


    /// <summary>
    ///     从UI根节点移除UI页面
    /// </summary>
    /// <param name="child">要移除的UI页面行为对象</param>
    public void RemoveUiPage(IUiPageBehavior child)
    {
        if (child.View is not Node node)
            return;

        node.GetParent()?.RemoveChild(node);
        _pages.Remove(child);
        node.QueueFreeX();
        _log.Debug($"Remove UI [{child.Key}]");
    }

    /// <summary>
    ///     Godot节点就绪时的回调方法
    ///     初始化UI层设置、绑定路由根节点，并切换到游戏主菜单状态
    /// </summary>
    public override void _Ready()
    {
        // 设置UI层级为UI根层
        Layer = UiLayers.UiRoot;
        InitLayers();
        var router = this.GetSystem<IUiRouter>()!;
        router.BindRoot(this);
        CallDeferred(nameof(CallDeferredCallback));
        _log.Debug($"[UiRoot] Ready. Path={GetPath()} InTree={IsInsideTree()}");
    }

    /// <summary>
    ///     延迟调用回调方法
    ///     发布UI根节点就绪事件，通知其他系统UI已准备完成
    /// </summary>
    private void CallDeferredCallback()
    {
        this.RunPublishCoroutine(new UiRootReadyEvent());
    }

    /// <summary>
    ///     初始化所有UI层容器
    ///     为每个UI层创建对应的Control容器节点
    /// </summary>
    private void InitLayers()
    {
        foreach (var layer in Enum.GetValues<UiLayer>())
        {
            var container = new Control
            {
                Name = layer.ToString(),
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 1,
                AnchorBottom = 1,
                MouseFilter = Control.MouseFilterEnum.Ignore
            };

            AddChild(container);
            _containers[layer] = container;
        }
    }
}