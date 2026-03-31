using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //设置为单例方便全局获取（懒加载，避免场景中忘记挂载导致为空）
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 尝试在场景中查找已有实例
                _instance = FindObjectOfType<InventoryManager>();
                if (_instance == null)
                {
                    // 场景中没有，则自动创建一个常驻对象
                    GameObject go = new GameObject("InventoryManager");
                    _instance = go.AddComponent<InventoryManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    //在这设置格子数量
    public int amountInventory = 50;
    //利用List存储虽然不如字典效率高，但是有天然的下标和格子匹配属性，且有一定排序逻辑
    public List<InventorySlot> slots = new List<InventorySlot>();
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
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
                        GameEvent.TriggerInventoryChanged();
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
                    empty.SetItem(item.MaxStack, item);
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
        GameEvent.TriggerInventoryChanged();
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
