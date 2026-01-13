using System;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI工厂类，用于创建和实例化UI页面
/// </summary>
/// <param name="registry">UI注册表，用于获取PackedScene类型的UI资源</param>
public class UiFactory(IUiRegistry<PackedScene> registry)
{
    /// <summary>
    /// 根据指定的UI键创建UI页面实例
    /// </summary>
    /// <param name="uiKey">UI资源的唯一标识键</param>
    /// <returns>实现IUiPage接口的UI页面实例</returns>
    /// <exception cref="InvalidCastException">当UI场景没有继承IUiPage接口时抛出</exception>
    public IUiPage Create(string uiKey)
    {
        // 从注册表中获取对应的场景资源
        var scene = registry.Get(uiKey);
        // 实例化场景节点
        var node  = scene.Instantiate();

        return node as IUiPage ?? throw new InvalidCastException($"UI scene {uiKey} must inherit IUiPage");
    }
}
