using System.Globalization;
using GFramework.Core.Abstractions.storage;
using GFramework.Core.utility;
using GFramework.Game.storage;
using GFrameworkGodotTemplate.scripts.data.interfaces;
using GFrameworkGodotTemplate.scripts.data.model;
using Godot;

namespace GFrameworkGodotTemplate.scripts.data;

/// <summary>
///     存档存储工具类，负责管理游戏存档的保存、加载、删除等操作
///     实现了ISaveStorageUtility接口，提供基于槽位的存档管理功能
/// </summary>
public class SaveStorageUtility : AbstractContextUtility, ISaveStorageUtility
{
    /// <summary>
    ///     获取存档槽位前缀配置项
    /// </summary>
    private static readonly string SaveSlotPrefix =
        ProjectSettings.GetSetting("application/config/save/save_slot_prefix").AsString();

    /// <summary>
    ///     存档文件夹的路径，保存在用户目录下的saves文件
    /// </summary>
    private static readonly string SaveRoot =
        ProjectSettings.GetSetting("application/config/save/save_path").AsString();


    private IStorage _rootStorage = null!;

    /// <summary>
    ///     获取存档文件路径
    /// </summary>
    private static string SaveFilePath { get; } =
        ProjectSettings.GetSetting("application/config/save/save_file_name").AsString();

    /// <summary>
    ///     检查指定槽位是否存在存档
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>如果存档存在返回true，否则返回false</returns>
    public bool Exists(int slot)
    {
        return SlotStorage(slot).Exists(SaveFilePath);
    }

    /// <summary>
    ///     加载指定槽位的存档数据
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>存档数据对象，如果不存在则返回新的GameSaveData实例</returns>
    public GameSaveData Load(int slot)
    {
        var storage = SlotStorage(slot);

        return storage.Exists(SaveFilePath)
            ? storage.Read<GameSaveData>(SaveFilePath)
            : new GameSaveData();
    }

    /// <summary>
    ///     保存数据到指定槽位
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <param name="data">要保存的游戏存档数据</param>
    public void Save(int slot, GameSaveData data)
    {
        data.RuntimeDirty = false;
        var slotDir = $"{SaveRoot}/{SaveSlotPrefix}{slot}";
        if (!DirAccess.DirExistsAbsolute(slotDir)) DirAccess.MakeDirRecursiveAbsolute(slotDir);

        SlotStorage(slot).Write(SaveFilePath, data);
    }

    /// <summary>
    ///     删除指定槽位的存档
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    public void Delete(int slot)
    {
        SlotStorage(slot).Delete(SaveFilePath);
    }

    /// <summary>
    ///     列出所有有效的存档槽位
    /// </summary>
    /// <returns>包含所有有效存档槽位编号的只读列表，按升序排列</returns>
    public IReadOnlyList<int> ListSlots()
    {
        // user://saves/slot_x 是否存在 save.json
        var dir = DirAccess.Open(SaveRoot);
        if (dir == null) return new List<int>();

        dir.ListDirBegin();
        var result = new List<int>();

        while (true)
        {
            var name = dir.GetNext();
            if (string.IsNullOrEmpty(name)) break;
            if (!dir.CurrentIsDir()) continue;
            if (!name.StartsWith(SaveSlotPrefix, StringComparison.Ordinal)) continue;
            if (!int.TryParse(name.AsSpan(SaveSlotPrefix.Length), CultureInfo.InvariantCulture, out var slot)) continue;
            var storage = SlotStorage(slot);
            if (storage.Exists(SaveFilePath))
                result.Add(slot);
        }

        dir.ListDirEnd();
        return result.OrderBy(x => x).ToList();
    }

    /// <summary>
    ///     初始化方法，设置根存储对象
    /// </summary>
    protected override void OnInit()
    {
        // 拿到全局 IStorage（GodotFileStorage）
        var baseStorage = this.GetUtility<IStorage>()!;
        // ✅ 确保存档根目录存在
        if (!DirAccess.DirExistsAbsolute(SaveRoot)) DirAccess.MakeDirRecursiveAbsolute(SaveRoot);

        _rootStorage = new ScopedStorage(baseStorage, SaveRoot);
    }

    /// <summary>
    ///     获取指定槽位的存储对象
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>对应槽位的存储对象</returns>
    private IStorage SlotStorage(int slot)
    {
        return new ScopedStorage(_rootStorage,
            string.Format(CultureInfo.InvariantCulture, "{0}{1}", SaveSlotPrefix, slot));
    }
}