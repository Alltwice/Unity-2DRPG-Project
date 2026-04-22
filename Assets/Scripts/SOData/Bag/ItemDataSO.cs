using UnityEngine;
using System;
using System.Collections.Generic;
public enum ItemType
{
    未知,
    武器,
    装备,
    消耗品,
    材料
}
public enum ItemRarity
{
    普通,
    稀有,
    史诗,
    传奇
}
[CreateAssetMenu(fileName = "NewBagData", menuName = "数据/物品数据", order = 2)]
public class ItemDataSO : ScriptableObject
{
    [Header("基础信息")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [TextArea(3, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemType itemType;
    // 兼容旧数据：若 `rarityWeights` 为空/权重无效，则将此字段当作“唯一结果”返回。
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private int prices;
    [Header("存放规则")]
    [SerializeField] private bool isStackable;
    [SerializeField] private int maxStack;
    public string ItemID => itemID;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public bool IsStackable => isStackable;
    public int MaxStack => maxStack;
    public ItemType ItemType => itemType;
    public int Prices => prices;

    [Serializable]
    public struct RarityWeight
    {
        public ItemRarity rarity;
        public int weight;
    }

    // 实例的品质由此处权重随机决定（背包按“每个数量单位”生成实例品质）。
    // 若列表为空或所有权重<=0，则退回使用 legacy `itemRarity` 作为唯一结果。
    [SerializeField] private List<RarityWeight> rarityWeights = new List<RarityWeight>();

    /// <summary>
    /// 玩大转盘
    /// </summary>
    public ItemRarity RollRarity()
    {
        if (rarityWeights == null || rarityWeights.Count == 0)
            return itemRarity;

        //计算出总权重
        int totalWeight = 0;
        for (int i = 0; i < rarityWeights.Count; i++)
        {
            int w = rarityWeights[i].weight;
            if (w > 0)
                totalWeight += w;
        }

        // 兜底：权重配置无效时，沿用旧字段。
        if (totalWeight <= 0)
            return itemRarity;
        //在总权重区域随机选取一点
        int roll = UnityEngine.Random.Range(0, totalWeight);
        int cursor = 0;
        for (int i = 0; i < rarityWeights.Count; i++)
        {
            var entry = rarityWeights[i];
            if (entry.weight <= 0)
                continue;
            cursor += entry.weight;
            if (roll < cursor)
                return entry.rarity;
        }

        // 理论上不会到这里，仍做兜底。
        return itemRarity;
    }

    // 兼容旧逻辑：仍保留该属性，供 UI/调试在未迁移完成前使用。
    public ItemRarity LegacyItemRarity => itemRarity;
    public virtual void UseMethod(GameObject targer, int index) { }
    public virtual void EquipMethod(GameObject target, int index) { }
    public virtual void UnequipMethod(GameObject target, TestEquipmentSlot slot) { }
}
