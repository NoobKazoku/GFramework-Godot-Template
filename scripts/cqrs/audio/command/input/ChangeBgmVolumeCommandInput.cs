using GFramework.Core.Abstractions.cqrs.command;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.command.input;

/// <summary>
///     背景音乐音量更改命令输入类，用于传递BGM音量更改所需的参数
/// </summary>
public sealed class ChangeBgmVolumeCommandInput : ICommandInput
{
    /// <summary>
    ///     获取或设置新的背景音乐音量值（0.0f - 1.0f）
    /// </summary>
    public float Volume { get; set; }
}