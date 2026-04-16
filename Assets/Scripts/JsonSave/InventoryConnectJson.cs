using UnityEngine;

/// <summary>
/// 该方法负责连接背包数据和JSON数据的转换
/// </summary>
public static class InventoryConnectJson
{
    /// <summary>
    /// 该方法在存入前调用负责将背包数据转换成JSON可存储的格式，比如记录ID和数量
    /// </summary>
    public static GameData BuildFromInventory(InventoryManager inv)
    {
        //构建背包
        inv.EnsureSlotCapacity();
        //构建储存数据的背包
        var save = new InventoryDto();
        foreach(InventorySlot slot in inv.slots)
        {
            var slotDto = new InventorySlotDto();
            //如果这个格子不为空
            if (slot != null && slot.instance != null && slot.amount > 0)
            {
                //存入数据
                slotDto.id = slot.instance.definition.ItemID;
                slotDto.rarity = (int)slot.instance.rarity;
                slotDto.amount = slot.amount;
            }
            else
            {
                //否则滞空
                slotDto.id = "";
                slotDto.amount = 0;
                slotDto.rarity = 0;
            }
            save.slotId.Add(slotDto);
        }
        //语法糖写法，新建内容后调用其中invnetory承接save并返回
        return new GameData
        {
            inventory = save
        };
    }
    /// <summary>
    /// 这个方法负责存档被取用后将JSON数据转换成背包数据然后让你看见
    /// </summary>
    public static void ApplyToInventory(GameData data, InventoryManager inv, BagItem catalog)
    {
        //如果数据为空就直接返回
        if (data == null)
        {
            return;
        }
        //如果目录为空就直接返回
        if (catalog == null)
        {
            return;
        }
        if(data.inventory.slotId==null)
        {
            inv.slots.Clear();
            GameEvent.TriggerInventoryChanged();
            return;
        }
        var saved = data.inventory.slotId;
        int savedCount = saved.Count;
        int targetSlots = Mathf.Max(savedCount, inv.amountInventory);
        inv.amountInventory = targetSlots;
        inv.EnsureSlotCapacity();

        for (int i = 0; i < inv.slots.Count; i++)
        {
            InventorySlot slot = inv.slots[i];
            if (slot == null)
                continue;

            if (i < savedCount)
            {
                InventorySlotDto dto = saved[i];
                if (dto == null || string.IsNullOrEmpty(dto.id) || dto.amount <= 0)
                {
                    slot.ClearItem();
                    continue;
                }
                if (catalog.TryGet(dto.id, out ItemDataSO itemSo))
                {
                    var instance = new ItemInstance(itemSo, (ItemRarity)dto.rarity);
                    slot.SetInstance(dto.amount, instance);
                }
                else
                {
                    Debug.LogWarning($"InventorySnapshotUtility: 未知 ItemID \"{dto.id}\"，已清空该槽位。");
                    slot.ClearItem();
                }
            }
            else
            {
                slot.ClearItem();
            }
        }

        GameEvent.TriggerInventoryChanged();
    }
}
