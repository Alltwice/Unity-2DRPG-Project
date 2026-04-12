using UnityEngine;

/// <summary>
/// 全局存档入口（测试用 MonoBehaviour）：不直接实现某一块业务逻辑，而是编排「收集 → 存盘 → 读盘 → 写回」。
/// <para><b>数据流（与其它脚本的关系）</b></para>
/// <list type="number">
/// <item><see cref="CaptureGlobalState"/>：从各系统拉快照，组装成一份 <see cref="GameData"/>（目前仅背包，见 <see cref="InventorySnapshotUtility.BuildFromInventory"/>）。</item>
/// <item><see cref="JsonPersistence.SaveToFile{T}"/>：把 <see cref="GameData"/> 写成 JSON 文件（与业务无关的纯 IO）。</item>
/// <item><see cref="JsonPersistence.TryLoadFromFile{T}"/>：从文件读出 <see cref="GameData"/>。</item>
/// <item><see cref="ApplyGlobalState"/>：把 <see cref="GameData"/> 按模块写回运行时（目前仅背包，见 <see cref="InventorySnapshotUtility.ApplyToInventory"/>；日后在此追加玩家属性、任务等）。</item>
/// </list>
/// 物品 id → <see cref="ItemDataSO"/> 的解析依赖 Inspector 中指定的 <see cref="BagItem"/> 目录资产。
/// </summary>
public class GameDataSaveLoadTester : MonoBehaviour
{
    [SerializeField]
    [Tooltip("读档时根据 ItemID 查找 ItemDataSO；保存快照本身不依赖目录，但完整读档流程需要。")]
    private BagItem itemCatalog;

    [SerializeField]
    [Tooltip("相对 Application.persistentDataPath 的路径；留空则使用 JsonProcess.DefaultSavePath")]
    private string relativeSavePath = JsonProcess.DefaultSavePath;

    // —— 对外 API：任意脚本也可直接调用下面静态方法，实现与 UI/测试组件解耦 ——

    /// <summary>
    /// 从当前运行中的各系统收集状态，得到一份完整的 <see cref="GameData"/>。
    /// 扩展方式：在此方法内合并各模块快照（例如先 new GameData()，再赋值 inventory、player 等），并配套在 <see cref="ApplyGlobalState"/> 里调用对应 Apply。
    /// </summary>
    public static GameData CaptureGlobalState()
    {
        // 当前唯一持久化模块：背包。若以后有玩家血量、任务列表等，在此组装进同一份 GameData。
        return InventorySnapshotUtility.BuildFromInventory(InventoryManager.Instance);
    }

    /// <summary>
    /// 将一份 <see cref="GameData"/> 写回运行时。需要 <paramref name="catalog"/> 以还原背包中的物品引用。
    /// </summary>
    public static void ApplyGlobalState(GameData data, BagItem catalog)
    {
        if (data == null)
        {
            Debug.LogWarning("GameDataSaveLoadTester.ApplyGlobalState: data 为 null。");
            return;
        }
        if (catalog == null)
        {
            Debug.LogError("GameDataSaveLoadTester.ApplyGlobalState: 未指定 BagItem，无法还原背包等物品引用。");
            return;
        }

        InventorySnapshotUtility.ApplyToInventory(data, InventoryManager.Instance, catalog);
        // 未来示例：PlayerSnapshotUtility.Apply(data.player, ...);
    }

    /// <summary>将 <see cref="CaptureGlobalState"/> 的结果（或你传入的整份 <see cref="GameData"/>）写入 JSON 文件。</summary>
    public static void SaveGlobalGameData(string pathOrRelative, GameData data = null)
    {
        GameData payload = data ?? CaptureGlobalState();
        JsonProcess.SaveToFile(payload, pathOrRelative);
    }

    /// <summary>从 JSON 文件读取 <see cref="GameData"/>；成功则通过 <see cref="ApplyGlobalState"/> 写回运行时。</summary>
    public static bool TryLoadGlobalGameData(string pathOrRelative, BagItem catalog, out GameData loaded)
    {
        loaded = null;
        if (!JsonProcess.TryLoadFromFile<GameData>(pathOrRelative, out GameData data) || data == null)
            return false;
        loaded = data;
        ApplyGlobalState(data, catalog);
        return true;
    }

    // —— Inspector 右键菜单：依赖本组件上序列化的路径与目录 ——

    [ContextMenu("保存全局存档 (GameData)")]
    public void SaveGame()
    {
        SaveGlobalGameData(relativeSavePath);
        Debug.Log($"全局存档已写入: {JsonProcess.ResolvePath(relativeSavePath)}");
    }

    [ContextMenu("读取全局存档 (GameData)")]
    public void LoadGame()
    {
        if (itemCatalog == null)
        {
            Debug.LogError("GameDataSaveLoadTester: 未指定 BagItem，无法完成读档（背包需要目录解析 ItemID）。");
            return;
        }
        if (!TryLoadGlobalGameData(relativeSavePath, itemCatalog, out _))
            Debug.LogWarning($"GameDataSaveLoadTester: 未找到或无法解析存档: {JsonProcess.ResolvePath(relativeSavePath)}");
        else
            Debug.Log("已从 JSON 加载并应用全局存档（当前含背包等已接入模块）。");
    }
}
