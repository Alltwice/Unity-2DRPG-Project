using System.Collections.Generic;
using System.Linq;

public static class InventoryQueryServiceTagBoost
{
    private static readonly Dictionary<ItemRarity, int> RarityWeights = new Dictionary<ItemRarity, int>
    {
        { ItemRarity.普通, 0 },
        { ItemRarity.稀有, 1 },
        { ItemRarity.史诗, 2 },
        { ItemRarity.传奇, 3 }
    };

    public static List<InventorySlot> SortAndBoost(
        List<InventorySlot> slots,
        ItemType? boostedType,
        InventorySortMode sortMode,
        InventorySortDirection sortDirection)
    {
        if (slots == null)
            return new List<InventorySlot>();
        if (slots.Count == 0)
            return new List<InventorySlot>(slots);

        bool boostEnabled = boostedType.HasValue;
        //第一遍排序打标签，筛出空内容
        var nonEmpty = slots
            .Select((slot, sourceIndex) => new { slot, sourceIndex })
            .Where(x => x.slot != null && x.slot.instance != null && x.slot.amount > 0)
            .Select(x => new
            {
                Slot = x.slot,
                SourceIndex = x.sourceIndex,
                ItemId = x.slot.instance.definition.ItemID ?? string.Empty,
                ItemType = x.slot.instance.ItemType,
                Price = NormalizePrice(x.slot.instance.Prices),
                RarityWeight = GetRarityWeight(x.slot.instance.rarity)
            });
        //第二遍给空格子打标签方便放到后面
        var empty = slots
            .Select((slot, sourceIndex) => new { slot, sourceIndex })
            .Where(x => x.slot == null || x.slot.instance == null || x.slot.amount <= 0)
            .OrderBy(x => x.sourceIndex)
            .Select(x => x.slot);
        //查看是否选择了传入标签
        int BoostKey(ItemType itemType)
        {
            if (!boostEnabled)
                return 0;
            return itemType == boostedType.Value ? 0 : 1;
        }
        //以下是三种排序方式，通过标签，价格，品质排序
        if (sortMode == InventorySortMode.ByTag)
        {
            var ordered = nonEmpty
                .OrderBy(x => BoostKey(x.ItemType))
                .ThenBy(x => x.ItemId)
                .ThenBy(x => x.SourceIndex);
            //注意这的Concat的效果是将空格拼接
            return ordered.Select(x => x.Slot).Concat(empty).ToList();
        }

        if (sortMode == InventorySortMode.ByPrice)
        {
            //单纯通过枚举自身代表的0，1操作顺序
            var ordered = sortDirection == InventorySortDirection.Ascending
                //降序排序，从100-50这样
                ? nonEmpty.OrderByDescending(x => x.Price).ThenByDescending(x => x.RarityWeight).ThenBy(x => x.ItemId).ThenBy(x => x.SourceIndex)
                //升序排序
                : nonEmpty.OrderBy(x => x.Price).ThenBy(x => x.RarityWeight).ThenBy(x => x.ItemId).ThenBy(x => x.SourceIndex);
            return ordered.Select(x => x.Slot).Concat(empty).ToList();
        }
        
        {
            var ordered = sortDirection == InventorySortDirection.Ascending
                ? nonEmpty.OrderByDescending(x => x.RarityWeight).ThenByDescending(x => x.Price).ThenBy(x => x.ItemId).ThenBy(x => x.SourceIndex)
                : nonEmpty.OrderBy(x => x.RarityWeight).ThenBy(x => x.Price).ThenBy(x => x.ItemId).ThenBy(x => x.SourceIndex);
            return ordered.Select(x => x.Slot).Concat(empty).ToList();
        }
    }

    private static int GetRarityWeight(ItemRarity rarity)
    {
        return RarityWeights.TryGetValue(rarity, out int weight) ? weight : 0;
    }

    private static int NormalizePrice(int rawPrice)
    {
        return rawPrice < 0 ? 0 : rawPrice;
    }
}

