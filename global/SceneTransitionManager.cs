using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.coroutine;
using GFramework.Core.coroutine.extensions;
using GFramework.Core.coroutine.instructions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace GFrameworkGodotTemplate.global;

/// <summary>
/// 场景过渡管理器，负责处理场景之间的平滑过渡效果。
/// 该类通过Shader材质实现过渡动画，并支持截图、参数设置和协程控制。
/// </summary>
[ContextAware]
[Log]
public partial class SceneTransitionManager : Node, IController
{
    /// <summary>
    /// Shader参数名称，用于控制过渡进度。
    /// </summary>
    private static readonly string Progress = "progress";

    /// <summary>
    /// 画布层节点，用于确保过渡效果显示在最上层。
    /// </summary>
    private CanvasLayer _canvasLayer = null!;

    /// <summary>
    /// 当前使用的Shader材质实例。
    /// </summary>
    private ShaderMaterial _material = null!;

    /// <summary>
    /// 场景过渡管理器的单例实例。
    /// </summary>
    public static SceneTransitionManager? Instance { get; private set; }

    /// <summary>
    /// 获取场景过渡矩形节点。
    /// </summary>
    private ColorRect SceneTransitionRect => GetNode<ColorRect>("%SceneTransitionRect");

    /// <summary>
    /// 标识当前是否正在执行场景过渡。
    /// </summary>
    public bool IsTransitioning { get; private set; }

    /// <summary>
    /// 节点初始化方法，在场景加载完成后调用。
    /// 初始化画布层、Shader材质以及相关参数。
    /// </summary>
    public override void _Ready()
    {
        Instance = this;

        _canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
        _canvasLayer.Layer = 100; // 确保在最上层

        // 创建材质的独立副本
        var originalMaterial = (ShaderMaterial)SceneTransitionRect.Material;
        _material = (ShaderMaterial)originalMaterial.Duplicate();
        SceneTransitionRect.Material = _material;

        // 确保初始状态完全隐藏
        SceneTransitionRect.Visible = false;
        SceneTransitionRect.Modulate = new Color(1, 1, 1); // 确保不透明度正常

        // 初始化shader参数
        _material.SetShaderParameter(Progress, 0.0f);

        _log.Debug($"SceneTransitionManager初始化: Shader={_material.Shader?.ResourcePath}");
    }

    /// <summary>
    /// 执行场景过渡的协程方法。
    /// 包括截图、场景切换、过渡动画和资源清理等步骤。
    /// </summary>
    /// <param name="onSwitch">场景切换逻辑的协程。</param>
    /// <param name="duration">过渡动画的持续时间（秒）。</param>
    /// <returns>协程指令枚举器。</returns>
    public IEnumerator<IYieldInstruction> PlayTransitionCoroutine(IEnumerator<IYieldInstruction> onSwitch,
        float duration = 0.6f)
    {
        if (IsTransitioning)
        {
            _log.Warn("已有过渡正在进行中，忽略新的过渡请求");
            yield break;
        }

        IsTransitioning = true;
        _log.Debug("=== 开始场景过渡 ===");

        // 1. 截图当前画面
        _log.Debug("步骤1: 捕获当前画面");
        var captureFromInstruction = CaptureScreenshot().AsCoroutineInstruction();
        yield return captureFromInstruction;
        var fromTexture = captureFromInstruction.Result;

        _log.Debug($"旧画面: {fromTexture.GetWidth()}x{fromTexture.GetHeight()}");

        // 2. 执行场景切换
        _log.Debug("步骤2: 执行场景切换");
        yield return new WaitForCoroutine(onSwitch);

        // 3. 等待新场景渲染完成
        _log.Debug("步骤3: 等待新场景渲染");
        yield return new WaitOneFrame();
        yield return new WaitOneFrame();

        // 4. 截图新画面
        _log.Debug("步骤4: 捕获新画面");
        var captureToInstruction = CaptureScreenshot().AsCoroutineInstruction();
        yield return captureToInstruction;
        var toTexture = captureToInstruction.Result;

        _log.Debug($"新画面: {toTexture.GetWidth()}x{toTexture.GetHeight()}");

        // 5. 设置shader参数
        _log.Debug("步骤5: 设置shader参数");
        var viewport = GetViewport();
        var viewportSize = viewport.GetVisibleRect().Size;

        _material.SetShaderParameter("from_tex", fromTexture);
        _material.SetShaderParameter("to_tex", toTexture);
        _material.SetShaderParameter("resolution", viewportSize);
        _material.SetShaderParameter(Progress, 0.0f);

        _log.Debug($"分辨率: {viewportSize}");

        // 6. 显示过渡层
        _log.Debug("步骤6: 显示过渡层");
        SceneTransitionRect.Visible = true;

        // 等待一帧确保显示
        yield return new WaitOneFrame();

        // 7. 执行过渡动画
        _log.Debug($"步骤7: 执行过渡动画 (时长: {duration}s)");
        yield return new WaitForCoroutine(TweenProgressCoroutine(0f, 1f, duration));

        // 8. 清理
        _log.Debug("步骤8: 清理资源");
        SceneTransitionRect.Visible = false;
        _material.SetShaderParameter(Progress, 0.0f);

        fromTexture.Dispose();
        toTexture.Dispose();

        IsTransitioning = false;
        _log.Debug("=== 场景过渡完成 ===");
    }

    /// <summary>
    /// 异步方法，用于捕获当前屏幕截图并返回图像纹理。
    /// 在截图过程中会临时隐藏过渡层以避免干扰。
    /// </summary>
    /// <returns>包含截图结果的图像纹理任务。</returns>
    private async Task<ImageTexture> CaptureScreenshot()
    {
        // 临时隐藏过渡层
        var wasVisible = SceneTransitionRect.Visible;
        SceneTransitionRect.Visible = false;

        // 等待渲染完成
        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

        // 获取视口纹理
        var viewport = GetViewport();
        var image = viewport.GetTexture().GetImage();

        // 恢复可见性
        SceneTransitionRect.Visible = wasVisible;

        // 转换为 ImageTexture
        var texture = ImageTexture.CreateFromImage(image);

        return texture;
    }

    /// <summary>
    /// 执行过渡进度的Tween动画协程。
    /// 通过修改Shader参数实现平滑的过渡效果。
    /// </summary>
    /// <param name="from">起始进度值。</param>
    /// <param name="to">目标进度值。</param>
    /// <param name="duration">动画持续时间（秒）。</param>
    /// <returns>协程指令枚举器。</returns>
    private IEnumerator<IYieldInstruction> TweenProgressCoroutine(float from, float to, float duration)
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetTrans(Tween.TransitionType.Cubic);

        // 创建一个标志来跟踪完成状态
        var tweenFinished = false;

        tween.TweenMethod(
            Callable.From<float>(v => { _material.SetShaderParameter(Progress, v); }),
            from,
            to,
            duration
        );

        // 连接完成信号
        tween.Finished += () => tweenFinished = true;
        // 等待Tween完成
        while (!tweenFinished)
        {
            yield return new WaitOneFrame();
        }

        _log.Debug($"Tween动画完成");
    }
}