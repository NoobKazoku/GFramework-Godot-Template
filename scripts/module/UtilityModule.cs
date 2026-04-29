using GFramework.Core.Pause;
using GFramework.Godot.Pause;
using GFrameworkGodotTemplate.global;
using GFrameworkGodotTemplate.scripts.config;
using GFrameworkGodotTemplate.scripts.data;
using GFrameworkGodotTemplate.scripts.data.model;
using GFrameworkGodotTemplate.scripts.utility;
using Godot;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
///     工具模块类，负责安装和管理游戏中的实用工具组件
/// </summary>
public class UtilityModule : IArchitectureModule
{
    /// <summary>
    ///     安装模块到指定的游戏架构中
    /// </summary>
    /// <param name="architecture">要安装模块的目标游戏架构实例</param>
    public void Install(IArchitecture architecture)
    {
        var pauseStackManager = new PauseStackManager();
        if (GameEntryPoint.Tree is { } tree)
            pauseStackManager.RegisterHandler(new GodotPauseHandler(tree));
        architecture.RegisterUtility(pauseStackManager);

        architecture.RegisterUtility(new GodotUiRegistry());
        architecture.RegisterUtility(new GodotSceneRegistry());
        architecture.RegisterUtility(new GodotTextureRegistry());
        architecture.RegisterUtility(new GodotPrefabRegistry());
        architecture.RegisterUtility(new GodotUiFactory());
        architecture.RegisterUtility(new GodotSceneFactory());
        architecture.RegisterUtility(new TemplateContentCatalog());
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
        architecture.RegisterUtility<ISaveRepository<GameSaveData>>(new SaveRepository<GameSaveData>(storage,
            new SaveConfiguration
            {
                SaveRoot = ProjectSettings.GetSetting("application/config/save/save_path").AsString(),
                SaveSlotPrefix = ProjectSettings.GetSetting("application/config/save/save_slot_prefix").AsString(),
                SaveFileName = ProjectSettings.GetSetting("application/config/save/save_file_name").AsString()
            }));
        architecture.RegisterUtility(new SaveStorageUtility());
    }
}
