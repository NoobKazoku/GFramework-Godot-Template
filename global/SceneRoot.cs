using GFramework.Core.extensions;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.extensions;
using GFramework.Godot.scene;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace GFrameworkGodotTemplate.global;

[Log]
[ContextAware]
public partial class SceneRoot : Node2D, ISceneRoot
{
    private Node? _current;
    private IGodotSceneRegistry _registry = null!;
    public Node? Current => _current;

    public void Replace(string key)
    {
        // 1. 卸载旧场景
        _current?.QueueFreeX();
        _current = null;

        // 2. 加载新场景
        var packed = _registry.Get(key);
        var scene = packed.Instantiate();

        // 3. 挂载
        AddChild(scene);
        _current = scene;
    }

    public void Unload()
    {
        _current?.QueueFree();
        _current = null;
    }

    public override void _Ready()
    {
        _registry = this.GetUtility<IGodotSceneRegistry>()!;
        var router = this.GetSystem<ISceneRouter>()!;
        router.BindRoot(this);
        this.SendEvent<SceneRootReadyEvent>();
    }

    public struct SceneRootReadyEvent;
}