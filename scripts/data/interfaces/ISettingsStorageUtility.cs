using GFramework.Core.Abstractions.utility;
using GFrameworkGodotTemplate.scripts.data.model;

namespace GFrameworkGodotTemplate.scripts.data.interfaces;

/// <summary>
/// 定义设置数据存储工具的接口，提供加载和保存设置数据的功能
/// </summary>
public interface ISettingsStorageUtility: IUtility
{
    /// <summary>
    /// 从存储中加载设置数据
    /// </summary>
    /// <returns>加载的设置数据对象</returns>
    public SettingsData Load();
    
    /// <summary>
    /// 将设置数据保存到存储中
    /// </summary>
    /// <param name="data">要保存的设置数据对象</param>
    public void Save(SettingsData data);
}
