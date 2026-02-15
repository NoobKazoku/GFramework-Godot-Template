using GFramework.Game.Abstractions.scene;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
/// 场景根节点类
/// 继承自Node2D并实现ISceneRoot接口
/// 负责管理场景行为和视图节点
/// </summary>
[Log]
[ContextAware]
public partial class SceneRoot : Node2D, ISceneRoot
{
    /// <summary>
    /// 存储场景行为列表
    /// </summary>
    private readonly List<ISceneBehavior> _scenes = [];

    /// <summary>
    /// 当前视图节点引用
    /// </summary>
    private Node? _currentView;

    /// <summary>
    /// 获取当前视图节点
    /// </summary>
    public Node? Current => _currentView;

    /// <summary>
    /// 添加场景到根节点
    /// </summary>
    /// <param name="scene">要添加的场景行为对象</param>
    /// <exception cref="InvalidOperationException">当场景行为未继承Godot Node时抛出异常</exception>
    public void AddScene(ISceneBehavior scene)
    {
        if (scene.Original is not Node node)
            throw new InvalidOperationException(
                $"SceneBehavior must inherit Godot Node. Key={scene.Key}");

        if (node.GetParent() == null)
            AddChild(node);
        else if (node.GetParent() != this)
            node.Reparent(this);

        _currentView = node;

        if (!_scenes.Contains(scene))
            _scenes.Add(scene);

        _log.Debug($"Add Scene [{scene.Key}]");
    }

    /// <summary>
    /// 从根节点移除场景
    /// </summary>
    /// <param name="scene">要移除的场景行为对象</param>
    public void RemoveScene(ISceneBehavior scene)
    {
        if (scene.Original is not Node node)
            return;

        node.GetParent()?.RemoveChild(node);
        _scenes.Remove(scene);

        node.QueueFreeX();

        if (_currentView == node)
            _currentView = null;

        _log.Debug($"Remove Scene [{scene.Key}]");
    }

    /// <summary>
    /// 节点准备就绪时调用
    /// 初始化场景路由器并发送准备完成事件
    /// </summary>
    public override void _Ready()
    {
        var router = this.GetSystem<ISceneRouter>()!;
        router.BindRoot(this);

        this.SendEvent<SceneRootReadyEvent>();
        _log.Debug($"[SceneRoot] Ready Path={GetPath()}");
    }

    /// <summary>
    /// 场景根节点准备完成事件结构体
    /// 用于通知系统场景根节点已准备就绪
    /// </summary>
    public struct SceneRootReadyEvent;
}