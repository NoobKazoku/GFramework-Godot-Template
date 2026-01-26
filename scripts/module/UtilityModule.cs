using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Game.serializer;
using GFramework.Godot.storage;
using GFramework.Godot.ui;
using GFrameworkGodotTemplate.scripts.setting;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
/// 工具模块类，负责安装和管理游戏中的实用工具组件
/// </summary>
public class UtilityModule : AbstractModule
{
    /// <summary>
    /// 安装模块到指定的游戏架构中
    /// </summary>
    /// <param name="architecture">要安装模块的目标游戏架构实例</param>
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterUtility(new GodotUiFactory());
        var jsonSerializer = new JsonSerializer();
        architecture.RegisterUtility(jsonSerializer);
        architecture.RegisterUtility(new GodotFileStorage(jsonSerializer));
        architecture.RegisterUtility(new SettingsStorageUtility());
    }
}