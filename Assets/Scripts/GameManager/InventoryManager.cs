using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //设置为单例方便全局获取（懒加载，避免场景中忘记挂载导致为空）
    //私有静态实例，用于存储单例实例
    private static InventoryManager _instance;
    //公共静态属性，用于获取单例实例
    public static InventoryManager Instance
    {
        get
        {
            //如果实例为空，则尝试在场景中查找已有实例
            if (_instance == null)
            {
                // 尝试在场景中查找已有实例
                _instance = FindAnyObjectByType<InventoryManager>();
                if (_instance == null)
                {
                    // 场景中没有，则自动创建一个常驻对象
                    GameObject go = new GameObject("InventoryManager");
                    //自动挂载管理器脚本
                    _instance = go.AddComponent<InventoryManager>();
                    DontDestroyOnLoad(go);
                }
            }
            _instance.EnsureSlotCapacity();
            return _instance;
        }
        //私有设置方法，用于设置单例实例
        private set
        {
            _instance = value;
        }
    }
    //在这设置格子数量
    public int amountInventory = 50;
    //利用List存储虽然不如字典效率高，但是有天然的下标和格子匹配属性，且有一定排序逻辑
    public List<InventorySlot> slots = new List<InventorySlot>();

    /// <summary>保证格子列表长度；避免 UI 在 Awake 顺序下早于本脚本初始化时 slots 仍为空。</summary>
    public void EnsureSlotCapacity()
    {
        while (slots.Count < amountInventory)
            slots.Add(new InventorySlot());
    }

    private void Awake()
    {
        //手动方法依然可用
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        EnsureSlotCapacity();
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
    public void RemoveItem(int itemIndex,int amount)
    {
        InventorySlot slot = slots[itemIndex];
        if(slot != null)
        {
            if(slot.amount>amount)
            {
                slot.amount -= amount;
            }
            else
            {
                slot.ClearItem();
            }
        }
        GameEvent.TriggerInventoryChanged();
    }
    //拖拽交换逻辑
    public void SwapItems(int from, int to)
    {
        //记录一下拖拽前的格子和拖拽后的格子
        InventorySlot slotFrom=slots[from];
        InventorySlot slotTo=slots[to];
        if (slotFrom == null || slotFrom.item == null || slotFrom.amount <= 0)
        {
            return;
        }
        if (from==to)
        {
            return;
        }
        //如果说格子都不为空且物品相同且可堆叠就直接叠加数量
        if (slotFrom!=null&&slotTo!=null)
        {
            if(slotFrom.item==slotTo.item&&slotFrom.item.IsStackable==true)
            {
                //这里计算剩下格子的数量
                int leftSpace=slotTo.item.MaxStack-slotTo.amount;
                //如果剩下格子有剩余
                if(leftSpace>0)
                {
                    //通过取剩下格子或者物品多少处理不同的情况
                    int amountToMove=Mathf.Min(slotFrom.amount,leftSpace);
                    slotTo.amount+=amountToMove;
                    slotFrom.amount-=amountToMove;
                    if(slotFrom.amount<=0)
                    {
                        slotFrom.ClearItem();
                    }
                }
                GameEvent.TriggerInventoryChanged();
                return;
            }
        }
        //如果在合法范围内
        if(from>=0&&from<slots.Count&&to>=0&&to<slots.Count)
        {
            //记录当前格子的物品和数量，换就完了
            ItemDataSO tempItem=slotFrom.item;
            int tempAmount=slotFrom.amount;
            slotFrom.item=slotTo.item;
            slotFrom.amount=slotTo.amount;
            slotTo.item=tempItem;
            slotTo.amount=tempAmount;
        }
        GameEvent.TriggerInventoryChanged();
    }
}
