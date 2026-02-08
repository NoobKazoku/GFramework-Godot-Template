using GFramework.Core.Abstractions.command;

namespace GFrameworkGodotTemplate.scripts.command.graphics.input;

/// <summary>
///     分辨率更改命令输入类，用于传递分辨率更改所需的参数
/// </summary>
public sealed class ChangeResolutionCommandInput : ICommandInput
{
    /// <summary>
    ///     获取或设置分辨率的宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    ///     获取或设置分辨率的高度
    /// </summary>
    public int Height { get; set; }
}