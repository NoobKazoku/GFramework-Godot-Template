using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFramework.Godot.setting;
using GFramework.Godot.setting.data;
using GFrameworkGodotTemplate.scripts.setting.interfaces;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
/// Godot模块实现类，负责安装和注册游戏中的各种模型
/// </summary>
public class ModelModule : AbstractModule
{
    /// <summary>
    /// 安装模块方法，向架构中注册ArenaModel和UnitModel模型
    /// </summary>
    /// <param name="architecture">游戏架构实例，用于注册模型</param>
    public override void Install(IArchitecture architecture)
    {
        var settingsDataRepository = architecture.Context.GetUtility<ISettingsDataRepository>()!;
        var settingsModel =
            architecture.RegisterModel(new SettingsModel<ISettingsDataRepository>(settingsDataRepository));
        settingsModel
            .RegisterApplicator(new GodotAudioSettings(settingsModel, new AudioBusMap()))
            .RegisterApplicator(new GodotGraphicsSettings(settingsModel))
            .RegisterApplicator(new GodotLocalizationSettings(settingsModel, new LocalizationMap()))
            ;
    }
}