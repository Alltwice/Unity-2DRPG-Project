using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 纯 LINQ 的背包“排序+标签优先(Boost)”查询服务：
/// - 选择标签后：该 ItemType 的物品排在更前面（和价格/品质排序叠加）
/// - 始终把空槽放到最后（实现从背包第一格开始填充）
/// </summary>
public static class InventoryQueryServiceTagBoost
{
    //readonly只有在申明和构造函数中能被修改
    //使用字典增加枚举权重方便日后扩展类型和便于排序
    //用了集合初始化的语法糖写法
    private static readonly Dictionary<ItemRarity, int> RarityWeights = new Dictionary<ItemRarity, int>
    {
        { ItemRarity.普通, 0 },
        { ItemRarity.稀有, 1 },
        { ItemRarity.史诗, 2 },
        { ItemRarity.传奇, 3 }
    };
    /// <summary>
    /// 这就是供外部调用的排序方法
    /// </summary>
    public static List<InventorySlot> SortAndBoost(
        List<InventorySlot> slots,
        ItemType? boostedType,
        InventorySortField sortField,
        InventorySortDirection sortDirection)   
    {
        if (slots == null)
            return new List<InventorySlot>();

        if (slots.Count == 0)
            return new List<InventorySlot>(slots);
        //查看是否传入了某个特定标签类型
        bool boostEnabled = boostedType.HasValue;
        //第一次LINQ查询，返回了复制后的格子数据
        var nonEmpty = slots
            //第一遍返回成新元素只提供了标签
            .Select((slot, sourceIndex) => new { slot, sourceIndex })
            //然后开始判空过滤处理
            .Where(x => x.slot != null && x.slot.instance != null && x.slot.amount > 0)
            //这里是真正需要返回的类型，获取了数据类型，第一次标注的下标等等内容
            //相当于返回了一个被打包后的新对象，这些数据都是为了排序而服务
            .Select(x => new
            {
                Slot = x.slot,
                SourceIndex = x.sourceIndex,
                ItemId = x.slot.instance.definition.ItemID ?? string.Empty,
                ItemType = x.slot.instance.ItemType,
                Price = NormalizePrice(x.slot.instance.Prices),
                RarityWeight = GetRarityWeight(x.slot.instance.rarity)
            });
        //第二遍LINQ，目标是挑出空气确保其排到后面去
        var empty = slots
            .Select((slot, sourceIndex) => new { slot, sourceIndex })
            .Where(x => x.slot == null || x.slot.instance == null || x.slot.amount <= 0)
            .OrderBy(x => x.sourceIndex)
            .Select(x => x.slot);

        // 利用01赋值增加权重Boosted=true 在前；Boosted=false 在后
        int BoostKey(ItemType itemType)
        {
            //检查是否启用了标签优先功能（!boostEnabled 代表玩家没选标签，看全部）
            if (!boostEnabled)
            {
                // 如果没选标签，所有人都是平等的，权重统一为 0
                return 0;
            }
            else
            {
                //  进入“标签优先”逻辑：判断当前物品是否符合玩家选中的标签
                if (itemType == boostedType.Value)
                {
                    // 如果是选中的类型（比如“武器”），赋予它最高优先级（权重为 0）
                    return 0;
                }
                else
                {
                    // 如果不是选中的类型，赋予它较低优先级（权重为 1）
                    return 1;
                }
            }
        }

        // 始终返回“非空在前 + 空槽在后”，从而 UI 自动从第 1 格开始填充。
        if (sortField == InventorySortField.Price)
        {
            // 你确认的语义：
            // - 正序=价格高->低；倒序=价格低->高
            if (sortDirection == InventorySortDirection.Ascending)
            {
                var ordered = nonEmpty
                    .OrderBy(x => BoostKey(x.ItemType))
                    .ThenByDescending(x => x.Price) // 高->低
                    .ThenByDescending(x => x.RarityWeight) // 品质作为次序：好->坏
                    .ThenBy(x => x.ItemId)
                    .ThenBy(x => x.SourceIndex);

                return ordered
                    .Select(x => x.Slot)
                    .Concat(empty)
                    .ToList();
            }
            else
            {
                var ordered = nonEmpty
                    .OrderBy(x => BoostKey(x.ItemType))
                    .ThenBy(x => x.Price) // 低->高
                    .ThenBy(x => x.RarityWeight) // 品质作为次序：坏->好
                    .ThenBy(x => x.ItemId)
                    .ThenBy(x => x.SourceIndex);

                return ordered
                    .Select(x => x.Slot)
                    .Concat(empty)
                    .ToList();
            }
        }
        else
        {
            // sortField == Rarity
            // 你确认的语义：
            // - 正序=品质好->坏；倒序=品质坏->好（传奇->普通 为好->坏）
            if (sortDirection == InventorySortDirection.Ascending)
            {
                var ordered = nonEmpty
                    .OrderBy(x => BoostKey(x.ItemType))
                    .ThenByDescending(x => x.RarityWeight) // 好->坏
                    .ThenByDescending(x => x.Price) // tie-break：价格高->低
                    .ThenBy(x => x.ItemId)
                    .ThenBy(x => x.SourceIndex);

                return ordered
                    .Select(x => x.Slot)
                    .Concat(empty)
                    .ToList();
            }
            else
            {
                var ordered = nonEmpty
                    .OrderBy(x => BoostKey(x.ItemType))
                    .ThenBy(x => x.RarityWeight) // 坏->好
                    .ThenBy(x => x.Price) // tie-break：价格低->高
                    .ThenBy(x => x.ItemId)
                    .ThenBy(x => x.SourceIndex);

                return ordered
                    .Select(x => x.Slot)
                    .Concat(empty)
                    .ToList();
            }
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

