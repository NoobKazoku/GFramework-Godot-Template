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
///     ModelModule 类继承自 AbstractModule，用于在架构中安装和注册模型。
///     该模块主要负责初始化设置相关的模型，并将其注册到架构中。
/// </summary>
public class ModelModule : AbstractModule
{
    /// <summary>
    ///     安装方法，用于将模型注册到指定的架构中。
    /// </summary>
    /// <param name="architecture">IArchitecture 接口实例，表示当前的应用程序架构。</param>
    public override void Install(IArchitecture architecture)
    {
        // 获取设置数据仓库的实例，用于后续模型的初始化
        var settingsDataRepository = architecture.Context.GetUtility<ISettingsDataRepository>()!;

        // 注册设置模型，并配置其应用器（Applicator）
        architecture.RegisterModel(
            new SettingsModel<ISettingsDataRepository>(new SettingDataLocationProvider(), settingsDataRepository)
                .Also(it =>
                {
                    // 注册音频设置应用器，用于处理音频相关配置
                    it.RegisterApplicator(new GodotAudioSettings(it, new AudioBusMap()))
                        // 注册图形设置应用器，用于处理图形相关配置
                        .RegisterApplicator(new GodotGraphicsSettings(it))
                        // 注册本地化设置应用器，用于处理语言和区域相关配置
                        .RegisterApplicator(new GodotLocalizationSettings(it, new LocalizationMap()));
                })
        );
    }
}