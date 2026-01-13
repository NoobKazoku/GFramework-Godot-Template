using System;
using System.Collections.Generic;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI注册表类，用于管理和注册UI场景
/// </summary>
public class UiRegistry : IWritableUiRegistry<PackedScene>
{
    /// <summary>
    /// 存储UI键值对的字典，键为UI标识符，值为对应的PackedScene对象
    /// </summary>
    private readonly Dictionary<string, PackedScene> _map = new(StringComparer.Ordinal);

    /// <summary>
    /// 注册UI场景到注册表中
    /// </summary>
    /// <param name="key">UI的唯一标识符</param>
    /// <param name="scene">要注册的PackedScene对象</param>
    /// <returns>返回当前UI注册表实例，支持链式调用</returns>
    public IWritableUiRegistry<PackedScene> Register(string key, PackedScene scene)
    {
        _map[key] = scene;
        return this;
    }

    /// <summary>
    /// 根据键获取已注册的UI场景
    /// </summary>
    /// <param name="uiKey">UI的唯一标识符</param>
    /// <exception cref="KeyNotFoundException">当指定的键未找到对应的UI场景时抛出异常</exception>
    /// <returns>对应的PackedScene对象</returns>
    public PackedScene Get(string uiKey)
    {
        return !_map.TryGetValue(uiKey, out var scene) ? throw new KeyNotFoundException($"UI not registered: {uiKey}") : scene;
    }
}
