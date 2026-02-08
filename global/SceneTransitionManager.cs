using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.coroutine;
using GFramework.Core.coroutine.extensions;
using GFramework.Core.coroutine.instructions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
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
    /// 获取或设置场景过渡管理器的单例实例。
    /// </summary>
    public static SceneTransitionManager? Instance { get; private set; }

    /// <summary>
    /// 获取用于显示过渡效果的 ColorRect 节点。
    /// </summary>
    private ColorRect SceneTransitionRect => GetNode<ColorRect>("%SceneTransitionRect");

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
    }


    /// <summary>
    /// 执行场景过渡的协程逻辑，包括捕获当前画面、执行过渡动画、切换场景以及捕获新画面。
    /// </summary>
    /// <param name="onSwitch">在场景切换时执行的协程逻辑。</param>
    /// <param name="duration">整个过渡过程的总持续时间（单位：秒），默认值为 0.6 秒。</param>
    /// <returns>返回一个可枚举的协程指令，用于控制过渡流程的执行。</returns>
    public IEnumerator<IYieldInstruction> PlayTransitionCoroutine(IEnumerator<IYieldInstruction> onSwitch,
        float duration = 0.6f)
    {
        IsTransitioning = true;
        SceneTransitionRect.Visible = true;
        // 1. 截图整个屏幕（包括所有 UI 层）
        var captureInstruction = CaptureScreenshot().AsCoroutineInstruction();
        yield return captureInstruction;
        var fromTexture = captureInstruction.Result;
        _material.SetShaderParameter("from_tex", fromTexture);

        // 2. 前半段动画（0 → 0.5，六边形遮盖旧画面）
        yield return TweenProgress(0f, 0.5f, duration * 0.5f).AsCoroutineInstruction();

        // 3. 执行实际切换（场景/UI）
        yield return new WaitForCoroutine(onSwitch);

        // 4. 等待一帧让新场景渲染
        yield return new WaitOneFrame();

        // 5. 截图新画面
        var toTextureInstruction = CaptureScreenshot().AsCoroutineInstruction();
        yield return toTextureInstruction;
        var toTexture = toTextureInstruction.Result;
        _material.SetShaderParameter("to_tex", toTexture);

        // 6. 后半段动画（0.5 → 1.0，显示新画面）
        yield return TweenProgress(0.5f, 1f, duration * 0.5f).AsCoroutineInstruction();

        // 7. 清理
        SceneTransitionRect.Visible = false;
        fromTexture.Dispose();
        toTexture.Dispose();
        IsTransitioning = false;
    }

    /// <summary>
    /// 捕获整个屏幕的截图（包括所有 UI 层和场景）
    /// </summary>
    private async Task<ImageTexture> CaptureScreenshot()
    {
        // 等待渲染完成
        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

        // 获取视口的纹理
        var viewport = GetViewport();
        var image = viewport.GetTexture().GetImage();

        // 转换为 ImageTexture
        var texture = ImageTexture.CreateFromImage(image);

        return texture;
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