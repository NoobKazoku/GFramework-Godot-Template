using GFramework.Core.command;
using GFramework.Core.extensions;
using GFrameworkGodotTemplate.scripts.data.interfaces;
using GFrameworkGodotTemplate.scripts.data.model;
using GFrameworkGodotTemplate.scripts.setting.interfaces;

namespace GFrameworkGodotTemplate.scripts.command.setting;

/// <summary>
/// 保存游戏设置命令类
/// 负责将当前游戏设置数据保存到存储中
/// </summary>
public sealed class SaveSettingsCommand : AbstractCommand
{
    /// <summary>
    /// 执行保存设置命令的主逻辑
    /// </summary>
    protected override void OnExecute()
    {
        // 获取设置模型和存储工具实例
        var model = this.GetModel<ISettingsModel>()!;
        var storage = this.GetUtility<ISettingsStorageUtility>()!;

        // 构建设置数据对象，包含音频和图形设置
        var data = new SettingsData
        {
            Audio = new AudioSettings
            {
                MasterVolume = model.Audio.MasterVolume,
                BgmVolume = model.Audio.BgmVolume,
                SfxVolume = model.Audio.SfxVolume,
            },
            Graphics = new GraphicsSettings
            {
                Fullscreen = model.Graphics.Fullscreen,
                ResolutionWidth = model.Graphics.ResolutionWidth,
                ResolutionHeight = model.Graphics.ResolutionHeight,
            },
        };

        // 将设置数据保存到存储中
        storage.Save(data);
    }
}
