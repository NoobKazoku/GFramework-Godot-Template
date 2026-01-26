using GFramework.Core.Abstractions.model;
using GFrameworkGodotTemplate.scripts.data.entities;

namespace GFrameworkGodotTemplate.scripts.setting.interfaces;

/// <summary>
/// 定义应用程序设置模型的接口，继承自IModel基础接口
/// 提供对图形设置和音频设置的访问
/// </summary>
public interface ISettingsModel: IModel
{
    /// <summary>
    /// 获取图形设置配置对象
    /// </summary>
    GraphicsSettings Graphics { get; }
    
    /// <summary>
    /// 获取音频设置配置对象
    /// </summary>
    AudioSettings Audio { get; }
    
    /// <summary>
    /// 获取当前设置的数据对象
    /// </summary>
    SettingsData GetSettingsData();
}
