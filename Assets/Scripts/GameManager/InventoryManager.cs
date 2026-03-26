using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //设置为单例方便全局获取
    public static InventoryManager Instance { get; private set; }
    //在这设置格子数量
    public int amountInventory = 20;
    //利用List存储虽然不如字典效率高，但是有天然的下标和格子匹配属性，且有一定排序逻辑
    public List<InventorySlot> slots = new List<InventorySlot>();
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //初始化的同时设置格子
        for(int i=0;i<amountInventory;i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    /// <summary>
    /// 一个增加物品的方法
    /// </summary>
    public bool AddItem(int addamount,ItemDataSO item)
    {
        //通过是否可堆叠判断
        if(item.IsStackable)
        {
            //如果可堆叠优先寻找相同种类匹配
            foreach(InventorySlot slot in slots)
            {
                if(slot.item==item&&slot.amount<item.MaxStack)
                {
                    //数量要单独拉出来计算
                    int spaceLeft = item.MaxStack - slot.amount; 
                    if(addamount<=spaceLeft)
                    {
                        //如果一个格子够就直接填
                        slot.amount += addamount;
                        return true;
                    }
                    else
                    {
                        //填不满就直接设满并计算数量
                        slot.amount = item.MaxStack;
                        addamount -= spaceLeft;
                    }
                }
            }
        }
        //然后没填满就一直去寻找下一个格子
        while (addamount > 0)
        {
            InventorySlot empty = FindEmpty();
            if (empty != null)
            {
                //依旧简单小计算
                if (item.IsStackable == true && addamount > item.MaxStack)
                {
                    empty.SetItem(addamount, item);
                    addamount -= item.MaxStack;
                }
                else
                {
                    empty.SetItem(addamount, item);
                    addamount = 0;
                }
            }
            //如果没有空格子了就填入失败
            else
            {
                return false;
            }
        }
        return true;
    }
    //一个辅助的寻找空格子的方法
    private InventorySlot FindEmpty()
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item==null&&slot.amount==0)
            {
                return slot;
            }
        }
        return null;
    }
}
