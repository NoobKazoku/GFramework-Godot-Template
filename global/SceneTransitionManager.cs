using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.coroutine;
using GFramework.Core.coroutine.extensions;
using GFramework.Core.coroutine.instructions;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.scene;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
/// 场景过渡管理器，负责处理场景之间的过渡动画。
/// 实现了 IController 接口，并继承自 Godot 的 Node 类。
/// 提供异步过渡功能，支持捕获当前场景、执行过渡动画以及切换场景。
/// </summary>
[ContextAware]
[Log]
public partial class SceneTransitionManager : Node, IController
{
    /// <summary>
    /// 过渡效果使用的着色器材质。
    /// </summary>
    private ShaderMaterial _material = null!;

    /// <summary>
    /// 场景路由器，用于管理场景的加载和切换。
    /// </summary>
    private ISceneRouter _sceneRouter = null!;

    /// <summary>
    /// 获取或设置场景过渡管理器的单例实例。
    /// </summary>
    public static SceneTransitionManager? Instance { get; private set; }

    /// <summary>
    /// 获取用于显示过渡效果的 ColorRect 节点。
    /// </summary>
    private ColorRect SceneTransitionRect => GetNode<ColorRect>("%SceneTransitionRect");

    /// <summary>
    /// 获取源视口，用于捕获当前场景的画面。
    /// </summary>
    private SubViewport FromViewport => GetNode<SubViewport>("%FromViewport");

    /// <summary>
    /// 获取目标视口，用于捕获新场景的画面。
    /// </summary>
    private SubViewport ToViewport => GetNode<SubViewport>("%ToViewport");

    /// <summary>
    /// 获取或设置是否正在进行场景过渡。
    /// </summary>
    public bool IsTransitioning { get; private set; }

    /// <summary>
    /// 节点准备就绪时的回调方法。
    /// 在节点添加到场景树后调用，初始化单例实例、材质和场景路由器。
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
        _material = (ShaderMaterial)SceneTransitionRect.Material;
        SceneTransitionRect.Visible = false;
        _sceneRouter = this.GetSystem<ISceneRouter>()!;
    }

    /// <summary>
    /// 异步播放场景过渡动画。
    /// 包括捕获当前场景、执行过渡动画、切换场景以及捕获新场景。
    /// </summary>
    /// <param name="onSwitch">在场景切换时执行的协程。</param>
    /// <param name="duration">过渡动画的总持续时间（秒）。</param>
    /// <returns>返回一个可等待的协程指令枚举。</returns>
    public IEnumerator<IYieldInstruction> PlayTransitionAsync(IEnumerator<IYieldInstruction> onSwitch,
        float duration = 0.6f)
    {
        // 捕获当前画面
        CaptureCurrentScene(FromViewport);

        // 执行过渡动画的第一阶段：从 0 到 0.5
        yield return TweenProgress(0f, 0.5f, duration * 0.5f).AsCoroutineInstruction();

        // 唯一一次“切”的机会：执行场景切换逻辑
        yield return new WaitForCoroutine(onSwitch);

        // 捕获新画面
        CaptureCurrentScene(ToViewport);

        // 执行过渡动画的第二阶段：从 0.5 到 1
        yield return TweenProgress(0.5f, 1f, duration * 0.5f).AsCoroutineInstruction();
    }

    /// <summary>
    /// 捕获指定视口中的当前场景画面。
    /// 清除视口中的现有子节点，并将当前场景的副本添加到视口中。
    /// </summary>
    /// <param name="viewport">要捕获画面的目标视口。</param>
    private void CaptureCurrentScene(SubViewport viewport)
    {
        foreach (var child in viewport.GetChildren())
        {
            child.QueueFreeX();
        }

        var sceneRouter = _sceneRouter as SceneRouter;

        var current = sceneRouter!.SceneRoot;
        if (current == null)
            return;
        var clone = current.Duplicate((int)DuplicateFlags.UseInstantiation);

        viewport.AddChild(clone);
    }

    /// <summary>
    /// 异步执行过渡进度的插值动画。
    /// 使用 Tween 方法平滑地更新着色器参数 "progress" 的值。
    /// </summary>
    /// <param name="from">起始进度值。</param>
    /// <param name="to">目标进度值。</param>
    /// <param name="duration">动画持续时间（秒）。</param>
    /// <returns>返回一个任务，表示动画完成。</returns>
    private async Task TweenProgress(float from, float to, float duration)
    {
        _material.SetShaderParameter("progress", from);

        var tween = CreateTween();
        tween.TweenMethod(
            Callable.From<float>(v => { _material.SetShaderParameter("progress", v); }),
            from,
            to,
            duration
        );

        await ToSignal(tween, Tween.SignalName.Finished);
    }
}