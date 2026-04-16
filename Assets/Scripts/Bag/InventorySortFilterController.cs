using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑层控制器：
/// 负责把 `InventoryManager.Instance.slots` 重排为“标签优先(Boost)+价格/品质排序+从第1格开始填充”。
/// </summary>
public static class InventorySortFilterController
{
    private static List<InventorySlot> _originalOrder;
    private static bool _hasOriginalOrder;

    public static void Apply(
        ItemType? boostedType,
        InventorySortField sortField,
        InventorySortDirection sortDirection)
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null)
            return;

        inv.EnsureSlotCapacity();

        // 只缓存一次：用于 Reset 回到“第一次 Apply 时”的原始槽位顺序。
        if (!_hasOriginalOrder || _originalOrder == null)
        {
            _originalOrder = new List<InventorySlot>(inv.slots);
            _hasOriginalOrder = true;
        }

        List<InventorySlot> newOrder = InventoryQueryServiceTagBoost.SortAndBoost(
            inv.slots,
            boostedType,
            sortField,
            sortDirection);

        // 保持 slots 列表对象引用，尽量减少潜在依赖问题。
        inv.slots.Clear();
        inv.slots.AddRange(newOrder);

        GameEvent.TriggerInventoryChanged();
    }

    public static void ResetOriginalOrder()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null)
            return;

        if (!_hasOriginalOrder || _originalOrder == null)
            return;

        inv.slots.Clear();
        inv.slots.AddRange(_originalOrder);

        GameEvent.TriggerInventoryChanged();
    }
}

