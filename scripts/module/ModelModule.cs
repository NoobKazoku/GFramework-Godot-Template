using GFramework.Core.Abstractions.architecture;
using GFramework.Core.functional.pipe;
using GFramework.Game.Abstractions.data;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFramework.Godot.setting;
using GFramework.Godot.setting.data;
using GFrameworkGodotTemplate.scripts.setting;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
///     Godot模块实现类，负责安装和注册游戏中的各种模型
/// </summary>
public class ModelModule : AbstractModule
{
    /// <summary>
    ///     安装模块方法，向架构中注册ArenaModel和UnitModel模型
    /// </summary>
    /// <param name="architecture">游戏架构实例，用于注册模型</param>
    public override void Install(IArchitecture architecture)
    {
        var settingsDataRepository = architecture.Context.GetUtility<ISettingsDataRepository>()!;
        architecture.RegisterModel(
            new SettingsModel<ISettingsDataRepository>(new SettingDataLocationProvider(), settingsDataRepository)
                .Also(it =>
                {
                    it.RegisterApplicator(new GodotAudioSettings(it, new AudioBusMap()))
                        .RegisterApplicator(new GodotGraphicsSettings(it))
                        .RegisterApplicator(new GodotLocalizationSettings(it, new LocalizationMap()));
                })
        );
    }
}