using System;
using GFramework.Core.Extensions;
using GFramework.Core.Utility;
using GFramework.SourceGenerators.Abstractions.Logging;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI工厂类，用于创建和实例化UI页面
/// </summary>
public partial class UiFactory:AbstractContextUtility, IUiFactory
{
    protected IWritableUiRegistry<PackedScene> Registry = null!;
    
    /// <summary>
    /// 根据指定的UI键创建UI页面实例
    /// </summary>
    /// <param name="uiKey">UI资源的唯一标识键</param>
    /// <returns>实现IUiPage接口的UI页面实例</returns>
    /// <exception cref="InvalidCastException">当UI场景没有继承IUiPage接口时抛出</exception>
    public IPageBehavior Create(string uiKey)
    {
        // 从注册表中获取对应的场景资源
        var scene = Registry.Get(uiKey);
        // 实例化场景节点
        var node  = scene.Instantiate();
        if (node is not IUiPageProvider provider)
            throw new InvalidCastException(
                $"UI scene {uiKey} must implement IUiPageProvider");
        return provider.GetPage();
    }

    /// <summary>
    /// 初始化方法，获取UI注册表实例
    /// </summary>
    protected override void OnInit()
    {
       Registry = this.GetUtility<IWritableUiRegistry<PackedScene>>()!;
    }
}
