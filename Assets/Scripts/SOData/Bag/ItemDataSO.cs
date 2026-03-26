using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewBagData", menuName = "数据/物品数据/", order = 2)]
public class ItemDataSO : ScriptableObject
{
    [Header("基础信息")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [TextArea(3, 5)]
    [SerializeField] private string description;
    [SerializeField] private Image icon;
    [Header("存放规则")]
    [SerializeField] private bool isStackable;
    [SerializeField] private int maxStack;
    public string ItemID => itemID;
    public string ItemName => itemName;
    public string Description => description;
    public Image Icon => icon;
    public bool IsStackable => isStackable;
    public int MaxStack => maxStack;
}
