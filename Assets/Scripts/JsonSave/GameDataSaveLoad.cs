using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// 这个脚本最大的作用就是连接连接器和Json保存工具，而自己提供一个可让外部调用的保存方法和接受全局数据
/// </summary>
public class GameDataSaveLoad : MonoBehaviour
{
    private static int _pendingLoadSlotIndex = -1;

    [SerializeField]
    [Tooltip("读档时根据 ItemID 查找 ItemDataSO；保存快照本身不依赖目录，但完整读档流程需要。")]
    private BagItem itemCatalog;

    [SerializeField]
    [Tooltip("相对 Application.persistentDataPath 的路径；留空则使用 JsonProcess.DefaultSavePath")]
    private string relativeSavePath = JsonProcess.DefaultSavePath;

    [SerializeField]
    [Tooltip("相对 persistentDataPath 的槽位目录。")]
    private string slotDirectory = JsonProcess.DefaultSlotDirectory;

    [SerializeField]
    [Tooltip("槽位文件名前缀。")]
    private string slotFilePrefix = JsonProcess.DefaultSlotFilePrefix;

    //检查是否存在之前存下的下标，如果有就加载
    private void Start()
    {
        if (_pendingLoadSlotIndex > 0)
        {
            int slotIndex = _pendingLoadSlotIndex;
            _pendingLoadSlotIndex = -1;
            TryLoadSlot(slotIndex, out _);
        }
    }
    /// <summary>
    /// 这个方法是调用所有连接器的数据主要作用是将游戏数据转化为Json可存储的数据，有多少模块需要保存就在这加多少
    /// 并且提供将数据转换为GameData方法即可
    /// </summary>
    /// <returns></returns>
    public static GameData CaptureGlobalState()
    {
        GameData data = InventoryConnectJson.BuildFromInventory(InventoryManager.Instance);
        if (data != null)
        {
            data.sceneName = SceneManager.GetActiveScene().name;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                PlayerSnapshotDto playerSnapshot = PlayerConnectJson.BuildFromPlayer(playerObj);
                if (playerSnapshot != null)
                    data.player = playerSnapshot;
            }
            else
            {
                Debug.LogWarning("GameDataSaveLoad: 未找到 Tag=Player 的对象，本次不写入玩家血量与位置。");
            }
        }
        return data;
    }
    /// <summary>
    /// 这里也是一个中间过程，主要作用是将Json数据转化为游戏数据并应用到游戏中
    /// 提供将存档数据赋值或转化给游戏对象的方法
    /// </summary>
    /// <param name="data"></param>
    /// <param name="catalog"></param>
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

        InventoryConnectJson.ApplyToInventory(data, InventoryManager.Instance, catalog);

        if (data.player == null)
        {
            Debug.LogWarning("GameDataSaveLoad: 存档中没有玩家状态字段（可能是旧存档），跳过玩家还原。");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogWarning("GameDataSaveLoad: 未找到 Tag=Player 的对象，跳过玩家血量与位置还原。");
            return;
        }

        PlayerConnectJson.ApplyToPlayer(data, playerObj);
    }
    /// <summary>
    /// 将数据传入json保存工具
    /// </summary>
    /// <param name="pathOrRelative"></param>
    /// <param name="data"></param>
    public static void SaveGlobalGameData(string pathOrRelative, GameData data = null)
    {
        GameData payload = data ?? CaptureGlobalState();
        JsonProcess.SaveToFile(payload, pathOrRelative);
    }
    /// <summary>
    /// 还原方法
    /// </summary>
    /// <param name="pathOrRelative"></param>
    /// <param name="catalog"></param>
    /// <param name="loaded"></param>
    /// <returns></returns>
    public static bool TryLoadGlobalGameData(string pathOrRelative, BagItem catalog, out GameData loaded)
    {
        loaded = null;
        if (!JsonProcess.TryLoadFromFile<GameData>(pathOrRelative, out GameData data) || data == null)
            return false;
        loaded = data;
        ApplyGlobalState(data, catalog);
        return true;
    }
    /// <summary>
    /// 通过传入下标自动拼接成不同的新存档
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public string GetSlotRelativePath(int slotIndex)
    {
        return JsonProcess.BuildSlotRelativePath(slotIndex, slotDirectory, slotFilePrefix);
    }
    /// <summary>
    /// 提供一个查找存档是否存在的方法
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public bool HasSlot(int slotIndex)
    {
        return JsonProcess.SlotExists(slotIndex, slotDirectory, slotFilePrefix);
    }
    /// <summary>
    /// 获取时间戳和存档标号的方法
    /// </summary>
    /// <returns></returns>
    public List<JsonProcess.SaveSlotMeta> GetSlotsMeta()
    {
        return JsonProcess.ListSlots(slotDirectory, slotFilePrefix);
    }
    /// <summary>
    /// 提供下标尝试存档
    /// </summary>
    /// <param name="slotIndex"></param>
    public void SaveSlot(int slotIndex)
    {
        string relativePath = GetSlotRelativePath(slotIndex);
        SaveGlobalGameData(relativePath);
        Debug.Log($"GameDataSaveLoad: 槽位 {slotIndex} 已保存 -> {JsonProcess.ResolvePath(relativePath)}");
    }
    /// <summary>
    /// 提供下标尝试读档
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="loaded"></param>
    /// <returns></returns>
    public bool TryLoadSlot(int slotIndex, out GameData loaded)
    {
        loaded = null;
        if (itemCatalog == null)
        {
            Debug.LogError("GameDataSaveLoad: 未指定 BagItem，无法完成槽位读档。");
            return false;
        }

        string relativePath = GetSlotRelativePath(slotIndex);
        if (!TryLoadGlobalGameData(relativePath, itemCatalog, out loaded))
        {
            Debug.LogWarning($"GameDataSaveLoad: 槽位 {slotIndex} 不存在或解析失败 -> {JsonProcess.ResolvePath(relativePath)}");
            return false;
        }

        Debug.Log($"GameDataSaveLoad: 槽位 {slotIndex} 读取成功。");
        return true;
    }

    public bool TryReadSlotRawData(int slotIndex, out GameData loaded)
    {
        loaded = null;
        string relativePath = GetSlotRelativePath(slotIndex);
        if (!JsonProcess.TryLoadFromFile<GameData>(relativePath, out GameData data) || data == null)
        {
            Debug.LogWarning($"GameDataSaveLoad: 槽位 {slotIndex} 不存在或解析失败 -> {JsonProcess.ResolvePath(relativePath)}");
            return false;
        }

        loaded = data;
        return true;
    }
    /// <summary>
    /// 传入下标删除存档
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public bool DeleteSlot(int slotIndex)
    {
        bool deleted = JsonProcess.DeleteSlotFile(slotIndex, slotDirectory, slotFilePrefix);
        if (deleted)
            Debug.Log($"GameDataSaveLoad: 槽位 {slotIndex} 已删除。");
        else
            Debug.LogWarning($"GameDataSaveLoad: 槽位 {slotIndex} 不存在，未执行删除。");

        return deleted;
    }

    public static void SetPendingLoadSlot(int slotIndex)
    {
        _pendingLoadSlotIndex = slotIndex;
    }
    /// <summary>
    /// 提供存档测试方法
    /// </summary>
    [ContextMenu("保存全局存档 (GameData)")]
    public void SaveGame()
    {
        SaveGlobalGameData(relativeSavePath);
        Debug.Log($"全局存档已写入: {JsonProcess.ResolvePath(relativeSavePath)}");
    }
    /// <summary>
    /// 提供读档测试方法
    /// </summary>
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