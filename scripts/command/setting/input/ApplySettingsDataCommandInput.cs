using GFramework.Core.Abstractions.command;
using GFrameworkGodotTemplate.scripts.data.model;

namespace GFrameworkGodotTemplate.scripts.command.setting.input;

/// <summary>
/// 应用设置数据命令输入类
/// 用于封装应用设置数据的命令输入参数
/// </summary>
public sealed class ApplySettingsDataCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置要应用的设置数据
    /// </summary>
    public SettingsData Settings { get; init; } = null!;
}
