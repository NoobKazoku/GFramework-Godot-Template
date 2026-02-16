using GFramework.Core.Abstractions.cqrs.command;

namespace GFrameworkGodotTemplate.scripts.cqrs.graphics.command.input;

/// <summary>
///     切换全屏命令输入类，用于传递全屏状态更改所需的参数
/// </summary>
public sealed class ToggleFullscreenCommandInput : ICommandInput
{
    /// <summary>
    ///     获取或设置是否全屏
    /// </summary>
    public bool Fullscreen { get; set; }
}