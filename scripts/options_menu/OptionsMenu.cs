using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.coroutine;
using GFramework.Core.Abstractions.events;
using GFramework.Core.coroutine.instructions;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.ui;
using GFramework.Game.setting.events;
using GFramework.Godot.coroutine;
using GFramework.Godot.extensions.signal;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.component;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.cqrs.audio.command;
using GFrameworkGodotTemplate.scripts.cqrs.audio.command.input;
using GFrameworkGodotTemplate.scripts.cqrs.graphics.command;
using GFrameworkGodotTemplate.scripts.cqrs.graphics.command.input;
using GFrameworkGodotTemplate.scripts.cqrs.setting.command;
using GFrameworkGodotTemplate.scripts.cqrs.setting.command.input;
using GFrameworkGodotTemplate.scripts.cqrs.setting.query;
using GFrameworkGodotTemplate.scripts.enums.ui;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.options_menu;

/// <summary>
///     选项设置界面控制器
///     负责处理游戏设置界面的UI逻辑，包括音量控制、分辨率和全屏模式设置
/// </summary>
[ContextAware]
[Log]
public partial class OptionsMenu : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    // 语言选项
    private readonly string[] _languages =
    [
        "简体中文",
        "English"
    ];

    // 分辨率选项
    private readonly Vector2I[] _resolutions =
    [
        new(1920, 1080),
        new(1366, 768),
        new(1280, 720),
        new(1024, 768)
    ];

    private bool _initializing;

    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IUiRouter _uiRouter = null!;

    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.OptionsMenu);

    /// <summary>
    ///     主音量控制容器
    /// </summary>
    private VolumeContainer MasterVolume => GetNode<VolumeContainer>("%MasterVolumeContainer");

    /// <summary>
    ///     背景音乐音量控制容器
    /// </summary>
    private VolumeContainer BgmVolume => GetNode<VolumeContainer>("%BgmVolumeContainer");

    /// <summary>
    ///     音效音量控制容器
    /// </summary>
    private VolumeContainer SfxVolume => GetNode<VolumeContainer>("%SfxVolumeContainer");

    /// <summary>
    ///     分辨率选择按钮
    /// </summary>
    private OptionButton ResolutionOptionButton => GetNode<OptionButton>("%ResolutionOptionButton");

    /// <summary>
    ///     全屏模式选择按钮
    /// </summary>
    private OptionButton FullscreenOptionButton => GetNode<OptionButton>("%FullscreenOptionButton");

    /// <summary>
    ///     语言选择按钮
    /// </summary>
    private OptionButton LanguageOptionButton => GetNode<OptionButton>("%LanguageOptionButton");

    /// <summary>
    ///     获取页面行为实例，如果不存在则创建新的CanvasItemUiPageBehavior实例
    /// </summary>
    /// <returns>返回IUiPageBehavior类型的页面行为实例</returns>
    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<Control>(this, UiKeyStr, UiLayer.Modal);
        return _page;
    }

    /// <summary>
    ///     检查当前UI是否在路由栈顶，如果不在则将页面推入路由栈
    /// </summary>
    private void CallDeferredInit()
    {
        InitCoroutine().RunCoroutine(segment: Segment.ProcessIgnorePause);
    }

    /// <summary>
    ///     节点准备就绪时的回调方法
    ///     在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _ = ReadyAsync();
    }

    private async Task ReadyAsync()
    {
        // 等待游戏架构初始化完成
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        GetNode<Button>("%Back").Pressed += OnBackPressed;
        SetupEventHandlers();
        // 获取UI路由器实例
        _uiRouter = this.GetSystem<IUiRouter>()!;
        // 延迟调用初始化方法
        CallDeferred(nameof(CallDeferredInit));
    }

    /// <summary>
    ///     处理未处理的输入事件，用于 ESC 关闭设置窗口
    /// </summary>
    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel")) return;

        OnBackPressed();
        AcceptEvent();
    }

    /// <summary>
    ///     当按下返回键时的处理方法
    /// </summary>
    private void OnBackPressed()
    {
        SaveCommandCoroutine().RunCoroutine(Segment.ProcessIgnorePause);
    }

    /// <summary>
    ///     开始保存设置协程
    /// </summary>
    private void StartSaving()
    {
        SaveCommandCoroutine().RunCoroutine(Segment.ProcessIgnorePause);
    }


    /// <summary>
    ///     异步初始化用户界面
    ///     设置音量控制组件和分辨率选项的初始值
    /// </summary>
    private async Task InitializeUiAsync()
    {
        _initializing = true;
        var view = await this.SendQueryAsync(new GetCurrentSettingsQuery()).ConfigureAwait(true);
        var audioSettings = view.Audio;
        MasterVolume.Initialize("主音量", audioSettings.MasterVolume);
        BgmVolume.Initialize("音乐音量", audioSettings.BgmVolume);
        SfxVolume.Initialize("音效音量", audioSettings.SfxVolume);

        var graphicsSettings = view.Graphics;
        ResolutionOptionButton.Disabled = graphicsSettings.Fullscreen;

        // 初始化全屏选项
        FullscreenOptionButton.Clear();
        FullscreenOptionButton.AddItem("全屏");
        FullscreenOptionButton.AddItem("窗口化");
        FullscreenOptionButton.Selected = graphicsSettings.Fullscreen ? 0 : 1;
        // 初始化分辨率选项
        ResolutionOptionButton.Clear();
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var r = _resolutions[i];
            ResolutionOptionButton.AddItem($"{r.X}x{r.Y}");

            if (r.X == graphicsSettings.ResolutionWidth && r.Y == graphicsSettings.ResolutionHeight)
                ResolutionOptionButton.Selected = i; // ⭐ 正确方式
        }

        var localizationSettings = view.Localization;
        LanguageOptionButton.Clear();
        LanguageOptionButton.AddItem("简体中文");
        LanguageOptionButton.AddItem("English");
        LanguageOptionButton.Selected =
            string.Equals(localizationSettings.Language, "简体中文", StringComparison.Ordinal) ? 0 : 1;
        _initializing = false;
    }

    /// <summary>
    ///     设置事件处理器
    ///     为音量控制、分辨率和全屏模式选择器绑定事件处理逻辑
    /// </summary>
    private void SetupEventHandlers()
    {
        var signalName = VolumeContainer.SignalName.VolumeChanged;
        MasterVolume
            .Signal(signalName)
            .To(Callable.From<float>(v =>
                this.RunCommandCoroutine(
                    new ChangeMasterVolumeCommand(new ChangeMasterVolumeCommandInput { Volume = v }))))
            .End();
        BgmVolume
            .Signal(signalName)
            .To(Callable.From<float>(v =>
                this.RunCommandCoroutine(
                    new ChangeBgmVolumeCommand(
                        new ChangeBgmVolumeCommandInput { Volume = v }))))
            .End();
        SfxVolume
            .Signal(signalName)
            .To(Callable.From<float>(v =>
                this.RunCommandCoroutine(
                    new ChangeSfxVolumeCommand(
                        new ChangeSfxVolumeCommandInput { Volume = v }))))
            .End();
        ResolutionOptionButton.ItemSelected += async index => await OnResolutionChanged(index).ConfigureAwait(false);
        FullscreenOptionButton.ItemSelected += async index => await OnFullscreenChanged(index).ConfigureAwait(false);
        LanguageOptionButton.ItemSelected += async index => await OnLanguageChanged(index).ConfigureAwait(false);
    }

    /// <summary>
    ///     语言改变事件
    /// </summary>
    /// <param name="index">选择的语言索引</param>
    private async Task OnLanguageChanged(long index)
    {
        if (_initializing) return;

        // 根据索引获取对应的语言
        var language = index == 0 ? "简体中文" : "English";

        // 发送更改语言命令
        await this.SendCommandAsync(new ChangeLanguageCommand(new ChangeLanguageCommandInput
            { Language = language })).ConfigureAwait(false);

        _log.Debug($"语言更改为: {language}");
    }

    /// <summary>
    ///     分辨率改变事件
    /// </summary>
    /// <param name="index">选择的分辨率索引</param>
    private async Task OnResolutionChanged(long index)
    {
        if (_initializing) return;
        var resolution = _resolutions[index];
        await this.SendCommandAsync(new ChangeResolutionCommand(new ChangeResolutionCommandInput
            { Width = resolution.X, Height = resolution.Y })).ConfigureAwait(false);
        _log.Debug($"分辨率更改为: {resolution.X}x{resolution.Y}");
    }

    /// <summary>
    ///     全屏模式改变事件
    /// </summary>
    /// <param name="index">选择的全屏模式索引</param>
    private async Task OnFullscreenChanged(long index)
    {
        var fullscreen = index == 0;
        await this.SendCommandAsync(new ToggleFullscreenCommand(new ToggleFullscreenCommandInput
            { Fullscreen = fullscreen })).ConfigureAwait(false);
        // ⭐ 禁用 / 启用分辨率选择
        ResolutionOptionButton.Disabled = fullscreen;
        _log.Debug($"全屏模式切换为: {fullscreen}");
    }

    /// <summary>
    ///     初始化协程，用于设置界面的初始化流程
    /// </summary>
    /// <returns>返回一个IYieldInstruction类型的IEnumerator，用于协程执行</returns>
    private IEnumerator<IYieldInstruction> InitCoroutine()
    {
        Hide();
        var settings = this.GetModel<ISettingsModel>()!;
        var eventBus = this.GetService<IEventBus>()!;
        if (!settings.IsInitialized)
            // 等待设置初始化事件完成
            yield return new WaitForEvent<SettingsInitializedEvent>(eventBus);

        yield return InitializeUiAsync().AsCoroutineInstruction();
        Show();
    }

    /// <summary>
    ///     保存命令协程，用于处理设置保存操作
    /// </summary>
    /// <returns>返回一个IYieldInstruction类型的IEnumerator，用于协程执行</returns>
    private IEnumerator<IYieldInstruction> SaveCommandCoroutine()
    {
        return this.SendCommandCoroutine(
                new SaveSettingsCommand(),
                e => _log.Error("保存失败！", e)
            )
            .Then(() =>
            {
                _log.Info("设置已保存");
                var handle = GetPage().Handle;
                if (handle.HasValue)
                    _uiRouter.Hide(handle.Value, UiLayer.Modal, true);
                else
                    _log.Warn("页面句柄为空，无法隐藏页面");
            });
    }
}