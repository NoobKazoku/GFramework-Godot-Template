using GFramework.Core.environment;
using GFrameworkGodotTemplate.scripts.constants;

namespace GFrameworkGodotTemplate.scripts.core.environment;

/// <summary>
///     游戏主环境类，继承自EnvironmentBase
///     负责游戏主环境的初始化和管理
/// </summary>
public class GameMainEnvironment : EnvironmentBase
{
    /// <summary>
    ///     获取环境名称
    /// </summary>
    public override string Name { get; } = GameConstants.Main;

    /// <summary>
    ///     初始化游戏主环境
    /// </summary>
    public override void Initialize()
    {
        // readme: 可以再此通过DevBindingManager.Instance拿取需要的测试对象
    }
}