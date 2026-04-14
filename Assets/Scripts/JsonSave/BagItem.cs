using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 负责存储背包数据和JSON数据
/// </summary>
[CreateAssetMenu(fileName = "ItemCatalog", menuName = "物品目录", order = 0)]
public class BagItem : ScriptableObject
{
    public List<ItemDataSO> data = new List<ItemDataSO>();
    private Dictionary<string, ItemDataSO> dataDic = new Dictionary<string, ItemDataSO>();
    private void OnEnable()
    {
        dataDic = null;
    }
    /// <summary>
    /// 存储信息方便查找
    /// </summary>
    public void BuildLookup()
    {
        #region agent log
        try
        {
            string logPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "debug-1496ca.log"));
            string line = "{\"sessionId\":\"1496ca\",\"hypothesisId\":\"A\",\"location\":\"BagItem.BuildLookup:entry\",\"message\":\"BuildLookup start\",\"data\":{\"dataDicIsNull\":" + (dataDic == null).ToString().ToLowerInvariant() + ",\"dataListIsNull\":" + (data == null).ToString().ToLowerInvariant() + ",\"dataCount\":" + (data != null ? data.Count : -1) + "},\"timestamp\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "}\n";
            File.AppendAllText(logPath, line);
        }
        catch { }
        #endregion
        if (dataDic == null)
            dataDic = new Dictionary<string, ItemDataSO>();
        else
            dataDic.Clear();
        foreach(ItemDataSO item in data)
        {
            #region agent log
            try
            {
                if (item == null)
                {
                    string logPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "debug-1496ca.log"));
                    string line = "{\"sessionId\":\"1496ca\",\"hypothesisId\":\"B\",\"location\":\"BagItem.BuildLookup:foreach\",\"message\":\"null item in data list\",\"data\":{},\"timestamp\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "}\n";
                    File.AppendAllText(logPath, line);
                }
            }
            catch { }
            #endregion
            if (item == null)
                continue;
            string id = item.ItemID;
            //或者物品id是空的就跳过这一轮循环
            if(string.IsNullOrEmpty(id))
            {
                continue;
            }
            #region agent log
            try
            {
                string logPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "debug-1496ca.log"));
                string line = "{\"sessionId\":\"1496ca\",\"hypothesisId\":\"A\",\"location\":\"BagItem.BuildLookup:beforeAssign\",\"message\":\"about to dataDic assign\",\"data\":{\"dataDicIsNull\":" + (dataDic == null).ToString().ToLowerInvariant() + ",\"idLen\":" + (id != null ? id.Length : -1) + "},\"timestamp\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "}\n";
                File.AppendAllText(logPath, line);
            }
            catch { }
            #endregion
            //如果存在就添加入字典
            dataDic[id] = item;
        }
    }
    /// <summary>
    /// 还原时可用，根据id获取物品数据
    /// </summary>
    public bool TryGet(string id, out ItemDataSO item)
    {
        item = null;
        if(string.IsNullOrEmpty(id))
        {
            return false;
        }
        #region agent log
        try
        {
            string logPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "debug-1496ca.log"));
            string line = "{\"sessionId\":\"1496ca\",\"hypothesisId\":\"D\",\"location\":\"BagItem.TryGet\",\"message\":\"TryGet\",\"data\":{\"dataDicIsNull\":" + (dataDic == null).ToString().ToLowerInvariant() + ",\"willBuildLookup\":" + (dataDic == null).ToString().ToLowerInvariant() + "},\"timestamp\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "}\n";
            File.AppendAllText(logPath, line);
        }
        catch { }
        #endregion
        if(dataDic==null)
        {
            BuildLookup();
        }
        return dataDic.TryGetValue(id, out item);
    }    
}
