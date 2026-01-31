using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.Game.setting.events;
using GFramework.Godot.extensions.signal;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.command.audio;
using GFrameworkGodotTemplate.scripts.command.audio.input;
using GFrameworkGodotTemplate.scripts.command.graphics;
using GFrameworkGodotTemplate.scripts.command.graphics.input;
using GFrameworkGodotTemplate.scripts.command.setting;
using GFrameworkGodotTemplate.scripts.component;
using GFrameworkGodotTemplate.scripts.options_menu.events;
using GFrameworkGodotTemplate.scripts.setting.query;
using Godot;

namespace GFrameworkGodotTemplate.scripts.options_menu;

/// <summary>
/// 选项设置界面控制器
/// 负责处理游戏设置界面的UI逻辑，包括音量控制、分辨率和全屏模式设置
/// </summary>
[ContextAware]
[Log]
public partial class OptionsMenu : Control, IController
{
	/// <summary>
	/// 主音量控制容器
	/// </summary>
	private VolumeContainer MasterVolume => GetNode<VolumeContainer>("%MasterVolumeContainer");

	/// <summary>
	/// 背景音乐音量控制容器
	/// </summary>
	private VolumeContainer BgmVolume => GetNode<VolumeContainer>("%BgmVolumeContainer");

	/// <summary>
	/// 音效音量控制容器
	/// </summary>
	private VolumeContainer SfxVolume => GetNode<VolumeContainer>("%SfxVolumeContainer");

	/// <summary>
	/// 分辨率选择按钮
	/// </summary>
	private OptionButton ResolutionOptionButton => GetNode<OptionButton>("%ResolutionOptionButton");

	/// <summary>
	/// 全屏模式选择按钮
	/// </summary>
	private OptionButton FullscreenOptionButton => GetNode<OptionButton>("%FullscreenOptionButton");
	
	/// <summary>
	/// 语言选择按钮
	/// </summary>
	private OptionButton LanguageOptionButton => GetNode<OptionButton>("%LanguageOptionButton");
	// 分辨率选项
	private readonly Vector2I[] _resolutions =
	[
		new(1920, 1080),
		new(1366, 768),
		new(1280, 720),
		new(1024, 768),
	];
	// 语言选项
	private readonly string[] _languages =
	[
		"简体中文",
		"English",
	];
	private bool _initializing;

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		Hide();
		this.RegisterEvent<SettingsInitializedEvent>(_ =>
		{
			InitializeUi();
			Show();
		});
		GetNode<Button>("%Back").Pressed += OnBackPressed;
		SetupEventHandlers();
	}

	/// <summary>
	/// 处理未处理的输入事件，用于 ESC 关闭设置窗口
	/// </summary>
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("ui_cancel"))
		{
			return;
		}
		OnBackPressed();
		AcceptEvent();

	}

	private void OnBackPressed()
	{
		this.SendCommand(new SaveSettingsCommand());
		_log.Info("设置已保存");
		this.SendEvent<CloseOptionsMenuEvent>();
	}

	/// <summary>
	/// 初始化用户界面
	/// 设置音量控制组件和分辨率选项的初始值
	/// </summary>
	private void InitializeUi()
	{
		_initializing = true;
		var view = this.SendQuery(new GetCurrentSettingsQuery());
		var audioSettings = view.Audio;
		MasterVolume.Initialize("主音量", audioSettings.MasterVolume);
		BgmVolume.Initialize("音乐音量", audioSettings.BgmVolume);
		SfxVolume.Initialize("音效音量", audioSettings.SfxVolume);
		// 初始化分辨率选项
		ResolutionOptionButton.Clear();
		foreach (var resolution in _resolutions)
		{
			ResolutionOptionButton.AddItem($"{resolution.X}x{resolution.Y}");
		}

		var graphicsSettings = view.Graphics;
		ResolutionOptionButton.Disabled = graphicsSettings.Fullscreen;
		
		// 初始化全屏选项
		FullscreenOptionButton.Clear();
		FullscreenOptionButton.AddItem("全屏");
		FullscreenOptionButton.AddItem("窗口化");
		FullscreenOptionButton.Selected = graphicsSettings.Fullscreen ? 0 : 1;
		for (var i = 0; i < _resolutions.Length; i++)
		{
			var r = _resolutions[i];
			ResolutionOptionButton.AddItem($"{r.X}x{r.Y}");

			if (r.X == graphicsSettings.ResolutionWidth && r.Y == graphicsSettings.ResolutionHeight)
			{
				ResolutionOptionButton.Selected = i; // ⭐ 正确方式
			}
		}

		var localizationSettings = view.Localization;
		LanguageOptionButton.Clear();
		LanguageOptionButton.AddItem("简体中文");
		LanguageOptionButton.AddItem("English");
		LanguageOptionButton.Selected = string.Equals(localizationSettings.Language, "简体中文", StringComparison.Ordinal) ? 0 : 1;
		_initializing = false;
	}

	/// <summary>
	/// 设置事件处理器
	/// 为音量控制、分辨率和全屏模式选择器绑定事件处理逻辑
	/// </summary>
	private void SetupEventHandlers()
	{
		var signalName = VolumeContainer.SignalName.VolumeChanged;
		MasterVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				_ = this.SendCommandAsync(new ChangeMasterVolumeCommand(
					new ChangeMasterVolumeCommandInput { Volume = v }))))
			.End();
		BgmVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				_ = this.SendCommandAsync(new ChangeBgmVolumeCommand(
					new ChangeBgmVolumeCommandInput { Volume = v }))))
			.End();
		SfxVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				_ = this.SendCommandAsync(new ChangeSfxVolumeCommand(
					new ChangeSfxVolumeCommandInput { Volume = v }))))
			.End();
		ResolutionOptionButton.ItemSelected += async index=> await OnResolutionChanged(index).ConfigureAwait(false);
		FullscreenOptionButton.ItemSelected += async index => await OnFullscreenChanged(index).ConfigureAwait(false);
	}

	/// <summary>
	/// 分辨率改变事件
	/// </summary>
	/// <param name="index">选择的分辨率索引</param>
	private async Task OnResolutionChanged(long index)
	{
		if (_initializing) return;
		var resolution = _resolutions[index]; 
		await ContextAwareExtensions.SendCommandAsync(this, new ChangeResolutionCommand(new ChangeResolutionCommandInput
			{ Width = resolution.X, Height = resolution.Y })).ConfigureAwait(false);
		_log.Debug($"分辨率更改为: {resolution.X}x{resolution.Y}");
	}

	/// <summary>
	/// 全屏模式改变事件
	/// </summary>
	/// <param name="index">选择的全屏模式索引</param>
	private async Task OnFullscreenChanged(long index)
	{
		var fullscreen = index == 0;
		await ContextAwareExtensions.SendCommandAsync(this, new ToggleFullscreenCommand(new ToggleFullscreenCommandInput { Fullscreen = fullscreen })).ConfigureAwait(false);
		// ⭐ 禁用 / 启用分辨率选择
		ResolutionOptionButton.Disabled = fullscreen;
		_log.Debug($"全屏模式切换为: {fullscreen}");
	}
}