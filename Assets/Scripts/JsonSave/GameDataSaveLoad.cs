using UnityEngine;

/// <summary>
/// 这个脚本最大的作用就是连接连接器和Json保存工具，而自己提供一个可让外部调用的保存方法和接受全局数据
/// </summary>
public class GameDataSaveLoad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("读档时根据 ItemID 查找 ItemDataSO；保存快照本身不依赖目录，但完整读档流程需要。")]
    private BagItem itemCatalog;

    [SerializeField]
    [Tooltip("相对 Application.persistentDataPath 的路径；留空则使用 JsonProcess.DefaultSavePath")]
    private string relativeSavePath = JsonProcess.DefaultSavePath;
    /// <summary>
    /// 这个方法是调用所有连接器的数据主要作用是将游戏数据转化为Json可存储的数据，有多少模块需要保存就在这加多少
    /// </summary>
    /// <returns></returns>
    public static GameData CaptureGlobalState()
    {
        return InventoryConnectJson.BuildFromInventory(InventoryManager.Instance);
    }
    /// <summary>
    /// 这里也是一个中间过程，主要作用是将Json数据转化为游戏数据并应用到游戏中，模块越多这里就越长
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
    //理论上上面两个方法已经可以用了，但是下面又包裹了一遍并提供了一些防御性编程处理，最终调用这两个方法 
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