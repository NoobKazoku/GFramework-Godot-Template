// 定义背景音乐类型的枚举

namespace GFrameworkGodotTemplate.scripts.enums.audio;

/// <summary>
///     背景音乐类型枚举，用于标识游戏中不同场景的背景音乐
/// </summary>
public enum BgmType
{
    /// <summary>
    ///     无背景音乐
    /// </summary>
    None,

    /// <summary>
    ///     主菜单背景音乐
    /// </summary>
    MainMenu,

    /// <summary>
    ///     游戏进行中背景音乐
    /// </summary>
    Gaming,

    /// <summary>
    ///     准备状态背景音乐
    /// </summary>
    Ready
}