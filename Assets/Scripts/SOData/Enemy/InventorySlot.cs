using System;
using UnityEngine;
[Serializable]
//MVC中的model层，模拟了背包中的一个格子，且为JSON准备
public class InventorySlot
{
    public int amount;
    public ItemDataSO item;
    //初始化构造
    public InventorySlot()
    {
        amount = 0;
        item = null;
    }
    //放入槽位
    public void SetItem(int amount,ItemDataSO item)
    {
        this.amount = amount;
        this.item = item;
    }
    //清空槽位
    public void ClearItem()
    {
        amount = 0;
        item = null;
    }
}
