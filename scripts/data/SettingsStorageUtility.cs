using GFramework.Core.Abstractions.storage;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using GFrameworkGodotTemplate.scripts.data.interfaces;
using GFrameworkGodotTemplate.scripts.data.model;
using Godot;

namespace GFrameworkGodotTemplate.scripts.data;

/// <summary>
/// 设置数据存储工具类，负责设置数据的加载和保存
/// </summary>
public class SettingsStorageUtility : AbstractContextUtility, ISettingsStorageUtility
{
    /// <summary>
    /// 存档文件的路径，保存在用户目录下的save.json文件
    /// </summary>
    private static readonly string Path = ProjectSettings.GetSetting("application/config/save/setting_path").AsString();

    private IStorage _storage = null!;

    /// <summary>
    /// 初始化存储工具，获取保存存储接口实例
    /// </summary>
    protected override void OnInit()
    {
        _storage = this.GetUtility<IStorage>()!;
    }

    /// <summary>
    /// 加载设置数据
    /// </summary>
    /// <returns>设置数据对象，如果文件不存在则返回新的默认设置数据</returns>
    public SettingsData Load()
    {
        return !_storage.Exists(Path) ? new SettingsData() : _storage.Read<SettingsData>(Path);
    }

    /// <summary>
    /// 保存设置数据到存储文件
    /// </summary>
    /// <param name="data">要保存的设置数据</param>
    public void Save(SettingsData data)
    {
        _storage.Write(Path, data);
    }
}