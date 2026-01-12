using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;

namespace GFrameworkTemplate.scripts.module;

/// <summary>
/// 系统Godot模块类，负责安装和注册游戏所需的各种系统组件
/// 继承自AbstractGodotModule，用于在游戏架构中集成系统功能
/// </summary>
public class SystemModule: AbstractModule
{
    /// <summary>
    /// 安装方法，用于向游戏架构注册各种系统组件
    /// </summary>
    /// <param name="architecture">游戏架构接口实例，用于注册系统</param>
    public override void Install(IArchitecture architecture)
    {

    }
}