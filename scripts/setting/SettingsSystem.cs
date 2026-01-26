using System.Threading.Tasks;
using GFramework.Core.extensions;
using GFramework.Core.system;
using GFramework.SourceGenerators.Abstractions.logging;
using GFrameworkGodotTemplate.scripts.constants;
using GFrameworkGodotTemplate.scripts.setting.interfaces;
using Godot;

namespace GFrameworkGodotTemplate.scripts.setting;

/// <summary>
/// 游戏设置系统，负责管理并应用游戏的各项设置（图形、音频等）
/// </summary>
[Log]
public partial class SettingsSystem : AbstractSystem, ISettingsSystem
{
    private ISettingsModel _settingsModel = null!;

    /// <summary>
    /// 应用所有设置（图形设置和音频设置）
    /// </summary>
    public async Task ApplyAll()
    {
        await ApplyGraphics().ConfigureAwait(false);
        ApplyAudio();
    }

    /// <summary>
    /// 应用图形设置，包括全屏模式和分辨率设置
    /// </summary>
    public async Task ApplyGraphics()
    {
        var g = _settingsModel.Graphics;
        var size = new Vector2I(g.ResolutionWidth, g.ResolutionHeight);

        // 直接调用DisplayServer API，不使用异步或延迟
        // 1. 设置边框标志
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, g.Fullscreen);

        // 2. 设置窗口模式
        DisplayServer.WindowSetMode(
            g.Fullscreen ? DisplayServer.WindowMode.ExclusiveFullscreen : DisplayServer.WindowMode.Windowed
        );

        // 3. 窗口化下设置尺寸和位置
        if (!g.Fullscreen)
        {
            DisplayServer.WindowSetSize(size);
            // 居中窗口
            var screen = DisplayServer.GetPrimaryScreen();
            var screenSize = DisplayServer.ScreenGetSize(screen);
            var pos = (screenSize - size) / 2;
            DisplayServer.WindowSetPosition(pos);
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>
    /// 应用音频设置，包括主音量、背景音乐音量和音效音量
    /// </summary>
    public void ApplyAudio()
    {
        var a = _settingsModel.Audio;
        SetBus(GameConstants.Master, a.MasterVolume);
        SetBus(GameConstants.Bgm, a.BgmVolume);
        SetBus(GameConstants.Sfx, a.SfxVolume);
    }

    /// <summary>
    /// 初始化设置系统，获取设置模型并应用所有设置
    /// </summary>
    protected override void OnInit()
    {
        _settingsModel = this.GetModel<ISettingsModel>()!;
    }

    /// <summary>
    /// 设置指定音频总线的音量
    /// </summary>
    /// <param name="name">音频总线名称</param>
    /// <param name="linear">线性音量值（0.0-1.0）</param>
    private static void SetBus(string name, float linear)
    {
        var idx = AudioServer.GetBusIndex(name);
        if (idx < 0)
        {
            _log.Warn($"音频总线未找到: {name}");
            return;
        }

        AudioServer.SetBusVolumeDb(
            idx,
            Mathf.LinearToDb(Mathf.Clamp(linear, 0.0001f, 1f))
        );
    }
}