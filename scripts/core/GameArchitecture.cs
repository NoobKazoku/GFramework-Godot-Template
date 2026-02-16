using GFramework.Core.Abstractions.architecture;
using GFramework.Core.Abstractions.environment;
using GFramework.Godot.architecture;
using GFrameworkGodotTemplate.scripts.module;
using Microsoft.Extensions.DependencyInjection;

namespace GFrameworkGodotTemplate.scripts.core;

/// <summary>
///     游戏架构类，负责安装和管理游戏所需的各种模块
///     继承自AbstractArchitecture，用于构建游戏的整体架构体系
/// </summary>
/// <param name="configuration">架构配置接口，用于配置架构的各项属性</param>
/// <param name="environment">环境接口，用于指定当前运行环境（如开发环境或生产环境）</param>
public sealed class GameArchitecture(IArchitectureConfiguration configuration, IEnvironment environment)
    : AbstractArchitecture(configuration, environment)
{
    public IArchitectureConfiguration Configuration { get; set; } = configuration;

    /// <summary>
    ///     配置服务集合的委托，用于注册中介者模式的相关服务
    ///     设置服务生命周期为单例模式
    /// </summary>
    public override Action<IServiceCollection> Configurator => collection =>
    {
        collection.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Singleton; });
    };

    /// <summary>
    ///     安装游戏所需的各个功能模块
    ///     该方法在架构初始化时被调用，用于注册系统、模型和工具模块
    /// </summary>
    protected override void InstallModules()
    {
        // 安装数据模型相关的Godot模块
        InstallModule(new ModelModule());
        // 安装系统相关的Godot模块
        InstallModule(new SystemModule());
        // 安装工具类相关的Godot模块
        InstallModule(new UtilityModule());
        // 安装状态相关的Godot模块
        InstallModule(new StateModule());
    }
}