using System;
using System.Collections.Generic;
using UnityEngine;

public static class TestEquipmentConnectJson
{
    //返回可存储的Json快照数据
    public static List<EquippedItemDto> BuildFromPlayer(GameObject playerObj)
    {
        if (playerObj == null)
            return new List<EquippedItemDto>();

        TestEquipmentManager manager = playerObj.GetComponent<TestEquipmentManager>();
        if (manager == null)
            return new List<EquippedItemDto>();

        return manager.BuildSnapshot();
    }

    public static void ApplyToPlayer(GameObject playerObj, List<EquippedItemDto> equipments, BagItem catalog)
    {
        if (playerObj == null || catalog == null)
            return;

        TestEquipmentManager manager = playerObj.GetComponent<TestEquipmentManager>();
        if (manager == null)
            manager = playerObj.AddComponent<TestEquipmentManager>();
        manager.ClearAllSlots();

        TestPlayerStatModifiers modifiers = playerObj.GetComponent<TestPlayerStatModifiers>();
        if (modifiers == null)
            modifiers = playerObj.AddComponent<TestPlayerStatModifiers>();
        modifiers.ClearAll();

        if (equipments == null)
            return;
        //拿到每一份数据后
        for (int i = 0; i < equipments.Count; i++)
        {
            EquippedItemDto dto = equipments[i];
            if (dto == null || string.IsNullOrEmpty(dto.slot) || string.IsNullOrEmpty(dto.itemId))
                continue;

            if (!Enum.TryParse(dto.slot, out TestEquipmentSlot slot))
                continue;
            #region agent log
            TestDebugSessionLogger.Log("pre-fix", "H4", "TestEquipmentConnectJson.ApplyToPlayer", "apply equipped dto", $"dtoSlot={dto.slot}, parsedSlot={slot}, itemId={dto.itemId}");
            #endregion
            //字典查找存储的物品下标返回具体物品
            if (!catalog.TryGet(dto.itemId, out ItemDataSO itemSo))
                continue;
            //实例化承接时用具体对象数据加记录中的稀有度数据
            ItemInstance instance = new ItemInstance(itemSo, (ItemRarity)dto.rarity);
            //装备上去，因为这里遍历的是装备格子
            manager.Equip(slot, instance);

            if (itemSo is TestEquipmentItemSO equipmentSo)
                equipmentSo.ApplyBonuses(modifiers);
        }
    }
}
