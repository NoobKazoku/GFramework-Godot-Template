using GFrameworkGodotTemplate.scripts.enums.audio;

namespace GFrameworkGodotTemplate.scripts.events.audio;

/// <summary>
///     音效播放事件类，用于触发特定类型的音效播放
/// </summary>
public class PlaySfxEvent
{
    /// <summary>
    ///     获取或设置要播放的音效类型
    /// </summary>
    public SfxType SfxType { get; set; }
}