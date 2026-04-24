using System.Collections.Generic;
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
        if (dataDic == null)
            dataDic = new Dictionary<string, ItemDataSO>();
        else
            dataDic.Clear();
        foreach(ItemDataSO item in data)
        {
            if (item == null)
                continue;
            string id = item.ItemID;
            //或者物品id是空的就跳过这一轮循环
            if(string.IsNullOrEmpty(id))
            {
                continue;
            }
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
        if(dataDic==null)
        {
            BuildLookup();
        }
        return dataDic.TryGetValue(id, out item);
    }    
}
