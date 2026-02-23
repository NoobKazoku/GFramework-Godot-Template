using GFramework.Core.Abstractions.architecture;
using GFramework.Core.functional.pipe;
using GFramework.Game.state;
using GFrameworkGodotTemplate.scripts.core.state.impls;

namespace GFrameworkGodotTemplate.scripts.module;

/// <summary>
///     状态模块类，负责安装和注册游戏状态系统
/// </summary>
public class StateModule : IArchitectureModule
{
    /// <summary>
    ///     安装模块方法，向架构中注册游戏状态机和相关状态
    /// </summary>
    /// <param name="architecture">游戏架构实例，用于注册状态系统</param>
    public void Install(IArchitecture architecture)
    {
        architecture.RegisterSystem(new GameStateMachineSystem().Also(it =>
        {
            it.Register(new MainMenuState())
                .Register(new PlayingState())
                .Register(new PausedState())
                .Register(new GameOverState())
                .Register(new BootStartState());
        }));
    }
}