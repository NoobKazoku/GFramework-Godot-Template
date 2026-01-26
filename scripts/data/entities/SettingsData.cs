namespace GFrameworkGodotTemplate.scripts.data.entities;

/// <summary>
/// 游戏设置数据类，包含音频和图形设置的配置信息
/// </summary>
public class SettingsData
{
    /// <summary>
    /// 音频设置配置对象，包含所有音频相关的设置选项
    /// </summary>
    public AudioSettings Audio { get; set; } = new();

    /// <summary>
    /// 图形设置配置对象，包含所有图形渲染相关的设置选项
    /// </summary>
    public GraphicsSettings Graphics { get; set; } = new();
    
    /// <summary>
    /// 本地化设置配置对象，包含所有语言和区域相关的设置选项
    /// </summary>
    public LocalizationSettings Localization { get; set; } = new();
}
