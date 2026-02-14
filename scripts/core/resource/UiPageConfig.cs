using GFramework.Core.Abstractions.bases;
using GFrameworkGodotTemplate.scripts.enums.ui;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.resource;

/// <summary>
///     UI场景配置资源类，用于存储UI界面的键值和对应的场景资源
/// </summary>
[GlobalClass]
public partial class UiPageConfig : Resource, IKeyValue<string, PackedScene>
{
    /// <summary>
    ///     获取或设置UI界面的唯一标识键
    /// </summary>
    [Export]
    public UiKey UiKey { get; private set; }

    /// <summary>
    ///     获取或设置UI界面对应的场景资源
    /// </summary>
    [Export]
    public PackedScene Scene { get; private set; } = null!;

    /// <summary>
    ///     获取UI键的名称作为键值对的键
    /// </summary>
    public string Key => UiKey.ToString();

    /// <summary>
    ///     获取场景资源作为键值对的值
    /// </summary>
    public PackedScene Value => Scene;
}