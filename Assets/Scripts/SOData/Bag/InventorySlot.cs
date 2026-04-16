using System;
using UnityEngine;
[Serializable]
//MVC中的model层，模拟了背包中的一个格子，且为JSON准备
public class InventorySlot
{
    public int amount;
    public ItemInstance instance;
    //初始化构造
    public InventorySlot()
    {
        amount = 0;
        instance = null;
    }

    /// <summary>
    /// 放入槽位（直接传入实例）。
    /// </summary>
    public void SetInstance(int amount, ItemInstance instance)
    {
        this.amount = amount;
        this.instance = instance;
    }

    /// <summary>
    /// 放入槽位（传入定义并让模板随机品质；主要用于迁移期）。
    /// </summary>
    public void SetInstance(int amount, ItemDataSO definition)
    {
        if (definition == null)
        {
            this.amount = 0;
            this.instance = null;
            return;
        }

        this.amount = amount;
        this.instance = new ItemInstance(definition, definition.RollRarity());
    }

    //清空槽位
    public void ClearItem()
    {
        amount = 0;
        instance = null;
    }
}
