using System;
using GFramework.Core.Abstractions.architecture;
using GFramework.Core.Abstractions.logging;
using GFramework.Core.Abstractions.properties;
using GFramework.Core.Abstractions.state;
using GFramework.Core.architecture;
using GFramework.Core.extensions;
using GFramework.Godot.logging;
using GFramework.Godot.scene;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.command.setting;
using GFrameworkGodotTemplate.scripts.command.setting.input;
using GFrameworkGodotTemplate.scripts.core;
using GFrameworkGodotTemplate.scripts.core.environment;
using GFrameworkGodotTemplate.scripts.core.resource;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.data.interfaces;
using GFrameworkGodotTemplate.scripts.enums.game;
using GFrameworkGodotTemplate.scripts.setting.interfaces;
using Godot;
using Godot.Collections;

namespace GFrameworkGodotTemplate.global;

/// <summary>
/// 游戏入口点节点类，负责初始化游戏架构和管理全局游戏状态
/// </summary>
[Log]
[ContextAware]
public partial class GameEntryPoint : Node
{
    private IGodotSceneRegistry _sceneRegistry = null!;
    private ISettingsModel _settingsModel = null!;
    private ISettingsStorageUtility _settingsStorageUtility = null!;
    private IGodotUiRegistry _uiRegistry = null!;
    public static IArchitecture Architecture { get; private set; } = null!;
    [Export] public bool IsDev { get; set; } = true;

    /// <summary>
    /// UI页面配置数组，包含所有可用的UI页面配置项
    /// </summary>
    /// <value>
    /// 存储UiPageConfig对象的数组集合
    /// </value>
    [Export]
    public Array<UiPageConfig> UiPageConfigs { get; set; } = null!;

    [Export] public Array<GameSceneConfig> GameSceneConfigs { get; set; } = null!;

    /// <summary>
    /// Godot引擎调用的节点就绪方法，在此方法中初始化游戏架构和相关组件
    /// </summary>
    public override void _Ready()
    {
        // 创建并初始化游戏架构实例
        // 配置架构的日志记录属性，设置Godot日志工厂提供程序并指定最低日志级别为调试级别
        // 然后初始化架构实例以准备游戏运行环境
        Architecture = new GameArchitecture(new ArchitectureConfiguration
        {
            LoggerProperties = new LoggerProperties
            {
                LoggerFactoryProvider = new GodotLoggerFactoryProvider
                {
                    MinLevel = LogLevel.Debug,
                },
            },
        }, IsDev ? new GameDevEnvironment() : new GameMainEnvironment());
        Architecture.Initialize();
        _settingsStorageUtility = this.GetUtility<ISettingsStorageUtility>()!;
        _settingsModel = this.GetModel<ISettingsModel>()!;
        var data = _settingsStorageUtility.Load();
        _ = this.SendCommandAsync(new ApplySettingsDataCommand(new ApplySettingsDataCommandInput
        {
            Settings = data,
        })).ConfigureAwait(false);
        _log.Info("设置已加载");
        _sceneRegistry = this.GetUtility<IGodotSceneRegistry>()!;
        _uiRegistry = this.GetUtility<IGodotUiRegistry>()!;
        // 注册所有游戏场景配置到场景注册表中
        foreach (var gameSceneConfig in GameSceneConfigs)
        {
            _sceneRegistry.Registry(gameSceneConfig);
        }

        // 注册所有UI页面配置到UI注册表中
        foreach (var uiPageConfig in UiPageConfigs)
        {
            _uiRegistry.Registry(uiPageConfig);
        }

        // 检查是否应该进入主菜单状态，如果是则注册UI根节点就绪事件来切换到主菜单状态
        if (ShouldEnterMainMenu())
        {
            this.RegisterEvent<UiRoot.UiRootReadyEvent>(_ =>
            {
                this.GetSystem<IStateMachineSystem>()!
                    .ChangeTo<MainMenuState>();
            });
        }

        _log.Debug("GameEntryPoint ready.");
    }

    /// <summary>
    /// 判断当前场景是否为主菜单场景，决定是否需要进入主菜单状态
    /// </summary>
    /// <returns>如果当前场景是主菜单场景则返回true，否则返回false</returns>
    private bool ShouldEnterMainMenu()
    {
        var tree = GetTree();
        var currentScene = tree.CurrentScene;

        if (currentScene == null)
            return false;

        var scenePath = currentScene.SceneFilePath;
        return string.Equals(scenePath, _sceneRegistry.Get(nameof(GameSceneKey.Main)).GetPath(),
            StringComparison.Ordinal);
    }

    /// <summary>
    /// 当节点从场景树中移除时调用，保存当前设置数据到存储
    /// </summary>
    public override void _ExitTree()
    {
        _settingsStorageUtility.Save(_settingsModel.GetSettingsData());
    }
}