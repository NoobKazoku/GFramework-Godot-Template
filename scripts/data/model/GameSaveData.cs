using System.Text.Json.Serialization;

namespace GFrameworkGodotTemplate.scripts.data.model;

/// <summary>
///     游戏存档数据类，用于存储游戏进度相关的序列化数据
///     包含版本控制、保存时间、槽位描述等基本信息
/// </summary>
public class GameSaveData
{
    /// <summary>
    ///     存档版本号，用于处理不同版本间的兼容性
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    ///     存档时间，记录最后保存的时间
    /// </summary>
    public DateTime SaveTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     槽位描述，用于在UI中显示额外信息
    /// </summary>
    public string SlotDescription { get; set; } = string.Empty;

    /// <summary>
    ///     运行时脏标记，用于标识存档数据是否被修改过
    ///     此属性在JSON序列化时会被忽略
    /// </summary>
    [JsonIgnore]
    public bool RuntimeDirty { get; set; }
}