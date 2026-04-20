using System;
using UnityEngine;

/// <summary>
/// 物品实例：把“可变属性”（当前为品质 ItemRarity）从模板数据中拆出来。
/// </summary>
[Serializable]
public class ItemInstance
{
    public ItemDataSO definition;
    public ItemRarity rarity;

    public ItemInstance(ItemDataSO definition, ItemRarity rarity)
    {
        this.definition = definition;
        this.rarity = rarity;
    }

    public bool IsValid => definition != null;

    public string ItemID => definition != null ? definition.ItemID : string.Empty;
    public string ItemName => definition != null ? definition.ItemName : string.Empty;
    public Sprite Icon => definition != null ? definition.Icon : null;
    public ItemType ItemType => definition != null ? definition.ItemType : ItemType.未知;
    public int BasePrice => definition != null ? definition.Prices : 0;
    public int Prices => RarityPriceCalculator.ToFinalPrice(BasePrice, rarity);
    public int MaxStack => definition != null ? definition.MaxStack : 0;
    public bool IsStackable => definition != null && definition.IsStackable;

    public override string ToString()
    {
        return $"ItemInstance({ItemID}, rarity={rarity})";
    }
}

