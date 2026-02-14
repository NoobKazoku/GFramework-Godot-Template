using GFramework.Core.Abstractions.bases;
using GFrameworkGodotTemplate.scripts.enums.scene;
using Godot;
using Godot.Collections;

namespace GFrameworkGodotTemplate.scripts.core.resource;

/// <summary>
///     游戏场景配置资源类，用于存储游戏场景的键值和对应的场景资源
/// </summary>
[GlobalClass]
public partial class SceneConfig : Resource, IKeyValue<string, PackedScene>
{
    /// <summary>
    ///     获取或设置游戏场景的唯一标识键
    /// </summary>
    [Export]
    public SceneKey SceneKey { get; private set; }

    /// <summary>
    /// Z轴深度（控制渲染顺序）
    /// </summary>
    [Export]
    public int ZIndex { get; private set; }

    /// <summary>
    ///     获取或设置游戏场景的打包场景资源
    /// </summary>
    [Export]
    public PackedScene Scene { get; private set; } = null!;

    /// <summary>
    /// 是否在加载时暂停其他场景
    /// </summary>
    [Export]
    public bool PauseOthers { get; private set; }

    /// <summary>
    /// 是否可复用（Hide后可Resume）
    /// </summary>
    [Export]
    public bool Reusable { get; private set; }

    /// <summary>
    /// 场景类型标签
    /// </summary>
    [Export]
    public Array<SceneTag> Tags { get; private set; } = [];

    /// <summary>
    ///     获取场景键的字符串表示形式
    /// </summary>
    public string Key => SceneKey.ToString();

    /// <summary>
    ///     获取场景资源值
    /// </summary>
    public PackedScene Value => Scene;
}