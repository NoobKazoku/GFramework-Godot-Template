using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.scene;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.scene;
using GFrameworkGodotTemplate.scripts.enums.scene;
using Godot;

namespace GFrameworkGodotTemplate.scripts.tests;

/// <summary>
///     主页场景控制器类
///     继承自Node2D节点，实现控制器、场景行为提供者和简单场景接口
///     负责管理主页场景的生命周期和行为
/// </summary>
[ContextAware]
[Log]
public partial class Home : Node2D, IController, ISceneBehaviorProvider, ISimpleScene
{
    /// <summary>
    ///     场景行为实例，用于管理具体的场景逻辑
    /// </summary>
    private ISceneBehavior? _scene;

    /// <summary>
    ///     获取场景键值字符串
    ///     返回枚举SceneKey.Home的名称作为场景标识
    /// </summary>
    public static string SceneKeyStr => nameof(SceneKey.Home);

    /// <summary>
    ///     获取场景行为实例
    ///     使用工厂模式创建场景行为，确保单例模式
    /// </summary>
    /// <returns>ISceneBehavior接口的场景行为实例</returns>
    public ISceneBehavior GetScene()
    {
        _scene ??= SceneBehaviorFactory.Create<Node2D>(this, SceneKeyStr);
        return _scene;
    }
}