using GFramework.Core.model;
using GFrameworkGodotTemplate.scripts.data.entities;
using GFrameworkGodotTemplate.scripts.setting.interfaces;

namespace GFrameworkGodotTemplate.scripts.setting;

/// <summary>
/// 设置模型类，继承自AbstractModel并实现ISettingsModel接口
/// 负责管理游戏的各种设置数据，包括图形、音频和本地化设置
/// </summary>
public class SettingsModel: AbstractModel,ISettingsModel
{
    /// <summary>
    /// 获取图形设置对象
    /// </summary>
    public GraphicsSettings Graphics { get; init; } = new();
    
    /// <summary>
    /// 获取音频设置对象
    /// </summary>
    public AudioSettings Audio { get; init; } = new();
    
    /// <summary>
    /// 获取本地化设置对象
    /// </summary>
    public LocalizationSettings Localization { get; init; } = new();
    
    /// <summary>
    /// 获取当前设置数据
    /// </summary>
    /// <returns>包含所有设置信息的SettingsData对象</returns>
    public SettingsData GetSettingsData()
    {
        return new SettingsData
        {
            Graphics = Graphics,
            Audio = Audio,
            Localization = Localization,
        };
    }

    /// <summary>
    /// 初始化方法，用于执行模型初始化逻辑
    /// </summary>
    protected override void OnInit()
    {
        
    }
}
