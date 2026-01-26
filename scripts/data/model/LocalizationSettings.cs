namespace GFrameworkGodotTemplate.scripts.data.model;

/// <summary>
/// 本地化设置类，用于管理游戏的语言本地化配置
/// </summary>
public sealed class LocalizationSettings
{
    /// <summary>
    /// 获取或设置当前使用的语言
    /// </summary>
    /// <value>默认值为"简体中文"</value>
    public string Language { get; set; } = "简体中文";
}
