using TMPro;
using UnityEngine;

public class TipsUIPanel : MonoBehaviour
{
    public static TipsUIPanel Instance { get; private set; }
    private CanvasGroup canvasGroup;
    [Header("基础信息")]
    [SerializeField] private string itemName;
    [TextArea(3, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private int prices;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI pricesText;
    public TextMeshProUGUI tpyeText;
    public TextMeshProUGUI RarityText;
    public TextMeshProUGUI amountText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowUIInfo(ItemDataSO itemData, int amount)
    {
        transform.SetAsLastSibling();
        canvasGroup.alpha = 1;
        itemName = itemData.ItemName;
        description = itemData.Description;
        itemType = itemData.ItemType;
        itemRarity = itemData.ItemRarity;
        prices = itemData.Prices;
        itemNameText.text = itemName;
        descriptionText.text = description;
        tpyeText.text = "种类:" + itemType.ToString();
        pricesText.text = "价格:" + prices.ToString();
        RarityText.text = "品质:" + itemRarity.ToString();
        if (amount > 1)
        {
            amountText.text = "x" + amount.ToString();

        }
        else
        {
            amountText.text = "";
        }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
