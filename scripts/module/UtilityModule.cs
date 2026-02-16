using GFramework.Core.Abstractions.architecture;
using GFramework.Game.Abstractions.data;
using GFramework.Game.architecture;
using GFramework.Game.data;
using GFramework.Game.serializer;
using GFramework.Godot.scene;
using GFramework.Godot.storage;
using GFramework.Godot.ui;
using GFrameworkGodotTemplate.scripts.data;
using GFrameworkGodotTemplate.scripts.utility;
using Godot;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
///     工具模块类，负责安装和管理游戏中的实用工具组件
/// </summary>
public class UtilityModule : AbstractModule
{
    /// <summary>
    ///     安装模块到指定的游戏架构中
    /// </summary>
    /// <param name="architecture">要安装模块的目标游戏架构实例</param>
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterUtility(new GodotUiRegistry());
        architecture.RegisterUtility(new GodotSceneRegistry());
        architecture.RegisterUtility(new GodotTextureRegistry());
        architecture.RegisterUtility(new GodotUiFactory());
        architecture.RegisterUtility(new GodotSceneFactory());
        var jsonSerializer = new JsonSerializer();
        architecture.RegisterUtility(jsonSerializer);
        var storage = new GodotFileStorage(jsonSerializer);
        architecture.RegisterUtility(storage);
        architecture.RegisterUtility(new UnifiedSettingsDataRepository(storage, jsonSerializer,
            new DataRepositoryOptions
            {
                BasePath = ProjectSettings.GetSetting("application/config/save/setting_path").AsString(),
                AutoBackup = true
            }));
        architecture.RegisterUtility(new SaveStorageUtility());
    }
}