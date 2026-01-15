using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Godot.ui;
using GFrameworkGodotTemplate.scripts.core.constants;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

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
        architecture.RegisterUtility(
            new GodotUiRegistry()
                .Register(UiKeys.MainMenu, GD.Load<PackedScene>("res://scenes/tests/ui/main_menu/test_main_menu.tscn"))
                .Register(UiKeys.Page1, GD.Load<PackedScene>("res://scenes/tests/ui/page_1.tscn"))
                .Register(UiKeys.Page2, GD.Load<PackedScene>("res://scenes/tests/ui/page_2.tscn"))
                .Register(UiKeys.Page3, GD.Load<PackedScene>("res://scenes/tests/ui/page_3.tscn"))
        );
        architecture.RegisterUtility(new GodotUiFactory());
    }
}