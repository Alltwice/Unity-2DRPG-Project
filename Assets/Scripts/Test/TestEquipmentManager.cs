using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 逻辑流转
/// </summary>
public class TestEquipmentManager : MonoBehaviour
{
    [Serializable]
    public class EquippedSlotEntry
    {
        public TestEquipmentSlot slot;
        public ItemInstance instance;
    }

    [SerializeField] private List<EquippedSlotEntry> initialSlots = new List<EquippedSlotEntry>();
    private readonly Dictionary<TestEquipmentSlot, ItemInstance> equipped = new Dictionary<TestEquipmentSlot, ItemInstance>();

    public IReadOnlyDictionary<TestEquipmentSlot, ItemInstance> Equipped => equipped;
    
    private void Awake()
    {
        //初始化时就存入所有键
        EnsureAllSlotsInitialized();
        //通过早存好的键赋值
        for (int i = 0; i < initialSlots.Count; i++)
        {
            EquippedSlotEntry entry = initialSlots[i];
            if (entry == null)
                continue;

            equipped[entry.slot] = entry.instance;
        }
    }

    public ItemInstance Equip(TestEquipmentSlot slot, ItemInstance newInstance)
    {
        EnsureAllSlotsInitialized();
        //用旧有数据承接现有数据
        ItemInstance old = equipped[slot];
        #region agent log
        TestDebugSessionLogger.Log("pre-fix", "H3", "TestEquipmentManager.Equip", "equip slot mutation", $"slot={slot}, oldItem={(old != null ? old.ItemID : "null")}, newItem={(newInstance != null ? newInstance.ItemID : "null")}");
        #endregion
        //现有数据上传入新数据
        equipped[slot] = newInstance;
        //返回旧有数据
        return old;
    }

    public ItemInstance Unequip(TestEquipmentSlot slot)
    {
        EnsureAllSlotsInitialized();
        //直接卸下来
        ItemInstance old = equipped[slot];
        equipped[slot] = null;
        return old;
    }

    public ItemInstance GetSlot(TestEquipmentSlot slot)
    {
        EnsureAllSlotsInitialized();
        return equipped[slot];
    }

    public void ClearAllSlots()
    {
        EnsureAllSlotsInitialized();
        Array slots = Enum.GetValues(typeof(TestEquipmentSlot));
        for (int i = 0; i < slots.Length; i++)
        {
            TestEquipmentSlot slot = (TestEquipmentSlot)slots.GetValue(i);
            equipped[slot] = null;
        }
    }
    /// <summary>
    /// 绑定快照，将字典内（装备槽上的数据转换为可存储数据）
    /// </summary>
    public List<EquippedItemDto> BuildSnapshot()
    {
        EnsureAllSlotsInitialized();
        var result = new List<EquippedItemDto>();
        foreach (KeyValuePair<TestEquipmentSlot, ItemInstance> pair in equipped)
        {
            if (pair.Value == null || pair.Value.definition == null)
                continue;

            result.Add(new EquippedItemDto
            {
                slot = pair.Key.ToString(),
                itemId = pair.Value.definition.ItemID,
                rarity = (int)pair.Value.rarity
            });
        }

        return result;
    }
    /// <summary>
    /// 辅助方法，一一遍历确保字典内存入了该枚举类型
    /// </summary>
    private void EnsureAllSlotsInitialized()
    {
        Array slots = Enum.GetValues(typeof(TestEquipmentSlot));
        for (int i = 0; i < slots.Length; i++)
        {
            TestEquipmentSlot slot = (TestEquipmentSlot)slots.GetValue(i);
            if (!equipped.ContainsKey(slot))
                equipped.Add(slot, null);
        }
    }
}
