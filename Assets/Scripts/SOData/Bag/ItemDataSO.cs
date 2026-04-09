using UnityEngine;
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
    public ItemRarity ItemRarity => itemRarity;
    public int Prices => prices;
    public virtual void UseMethod(GameObject targer, int index) { }
}
