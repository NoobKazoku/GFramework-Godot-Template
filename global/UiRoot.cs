using GFramework.Core.extensions;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.constants;
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
    ///     设置UI页面的Z轴顺序
    /// </summary>
    /// <param name="page">目标UI页面</param>
    /// <param name="zOrder">Z轴排序值</param>
    public void SetZOrder(IUiPageBehavior page, int zOrder)
    {
        if (page.View is not CanvasItem item)
            return;

        // 查找页面所在的UI层
        var layer = _containers
            .FirstOrDefault(p => item.GetParent() == p.Value)
            .Key;

        item.ZIndex = (int)layer * 100 + zOrder;
        item.ZAsRelative = false;
    }

    /// <summary>
    ///     获取当前可见的UI页面列表
    /// </summary>
    /// <returns>可见UI页面的只读列表</returns>
    public IReadOnlyList<IUiPageBehavior> GetVisiblePages()
    {
        return _pages
            .Where(p => p.View is CanvasItem { Visible: true })
            .ToList();
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
        this.SendEvent<UiRootReadyEvent>();
        _log.Debug($"[UiRoot] Ready. Path={GetPath()} InTree={IsInsideTree()}");
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


    public struct UiRootReadyEvent;
}