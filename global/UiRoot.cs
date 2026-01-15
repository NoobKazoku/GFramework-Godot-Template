using System;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.constants;
using GFrameworkGodotTemplate.scripts.core.state;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
/// UI画布层根节点，用于管理UI页面的添加和组织
/// 继承自CanvasLayer并实现IUiRoot接口
/// </summary>
[Log]
[ContextAware]
public partial class UiRoot : CanvasLayer, IUiRoot
{
    public struct UiRootReadyEvent;

    /// <summary>
    /// Godot节点就绪时的回调方法
    /// 初始化UI层设置、绑定路由根节点，并切换到游戏主菜单状态
    /// </summary>
    public override void _Ready()
    {
        // 设置UI层级为UI根层
        Layer = UiLayers.UiRoot;

        var router = this.GetSystem<IUiRouter>()!;
        router.BindRoot(this);
        this.SendEvent<UiRootReadyEvent>();
    }

    /// <summary>
    /// 向UI根节点添加UI页面
    /// </summary>
    /// <param name="child">要添加的UI页面行为对象</param>
    public void AddUiPage(IUiPageBehavior child)
    {
        if (child.View is Node node)
            AddChild(node);
        else
            throw new InvalidOperationException("UIPage View must be a Godot Node");
    }

    /// <summary>
    /// 从UI根节点移除UI页面
    /// </summary>
    /// <param name="child">要移除的UI页面行为对象</param>
    public void RemoveUiPage(IUiPageBehavior child)
    {
        if (child.View is Node node)
            RemoveChild(node);
    }
}