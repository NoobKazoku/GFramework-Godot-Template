namespace GFrameworkGodotTemplate.scripts.enums.settings;

/// <summary>
///     定义设置更改的原因枚举
///     用于标识哪些设置类别发生了变化，以便进行相应的处理
/// </summary>
public enum SettingsChangedReason
{
    /// <summary>
    ///     音频设置发生了更改
    /// </summary>
    Audio,

    /// <summary>
    ///     图形设置发生了更改
    /// </summary>
    Graphics,

    /// <summary>
    ///     所有设置都发生了更改
    /// </summary>
    All
}