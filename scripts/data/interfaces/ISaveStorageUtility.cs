using GFrameworkGodotTemplate.scripts.data.model;

namespace GFrameworkGodotTemplate.scripts.data.interfaces;

/// <summary>
///     定义游戏存档存储工具的接口，提供存档的基本操作功能
/// </summary>
public interface ISaveStorageUtility
{
    /// <summary>
    ///     检查指定槽位是否存在存档
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>如果槽位存在存档则返回true，否则返回false</returns>
    bool Exists(int slot);

    /// <summary>
    ///     从指定槽位加载存档数据
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>存档数据对象</returns>
    GameSaveData Load(int slot);

    /// <summary>
    ///     将存档数据保存到指定槽位
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <param name="data">要保存的存档数据</param>
    void Save(int slot, GameSaveData data);

    /// <summary>
    ///     删除指定槽位的存档
    /// </summary>
    /// <param name="slot">要删除的存档槽位编号</param>
    void Delete(int slot);

    /// <summary>
    ///     获取所有可用存档槽位的列表
    /// </summary>
    /// <returns>包含所有存档槽位编号的只读列表</returns>
    IReadOnlyList<int> ListSlots();
}