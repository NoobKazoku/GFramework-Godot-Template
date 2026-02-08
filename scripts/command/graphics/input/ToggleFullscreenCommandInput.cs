using GFramework.Core.Abstractions.command;

namespace GFrameworkGodotTemplate.scripts.command.graphics.input;

/// <summary>
///     切换全屏命令输入类
/// </summary>
public sealed class ToggleFullscreenCommandInput : ICommandInput
{
    /// <summary>
    ///     全屏状态属性
    /// </summary>
    public bool Fullscreen { get; set; }
}