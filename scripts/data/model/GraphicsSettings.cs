namespace GFrameworkGodotTemplate.scripts.data.model;

/// <summary>
/// 图形设置类，用于管理游戏的图形相关配置
/// </summary>
public sealed class GraphicsSettings
{
    /// <summary>
    /// 获取或设置是否启用全屏模式
    /// </summary>
    public bool Fullscreen { get; set; }
    
    /// <summary>
    /// 获取或设置屏幕分辨率宽度
    /// </summary>
    public int ResolutionWidth { get; set; } = 1920;
    
    /// <summary>
    /// 获取或设置屏幕分辨率高度
    /// </summary>
    public int ResolutionHeight { get; set; } = 1080;
}
