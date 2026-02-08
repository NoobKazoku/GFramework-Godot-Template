using GFramework.Core.environment;
using GFrameworkGodotTemplate.scripts.constants;

namespace GFrameworkGodotTemplate.scripts.core.environment;

/// <summary>
///     游戏开发环境类，继承自EnvironmentBase
///     用于管理开发环境下的特定资源和配置
/// </summary>
public class GameDevEnvironment : EnvironmentBase
{
    /// <summary>
    ///     获取环境名称
    /// </summary>
    public override string Name { get; } = GameConstants.Development;

    /// <summary>
    ///     初始化开发环境
    ///     从开发绑定管理器中获取玩家数据资源并进行初始化
    /// </summary>
    public override void Initialize()
    {
        // readme: 可以再此通过DevBindingManager.Instance拿取需要的测试对象
    }
}