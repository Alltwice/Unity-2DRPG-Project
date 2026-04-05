using TMPro;
using UnityEngine;

public class TipsUIPanel : MonoBehaviour
{
    public static TipsUIPanel Instance { get; private set; }
    private CanvasGroup canvasGroup;
    [Header("基础信息")]
    [SerializeField] private string itemID;
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

    public void ShowUIInfo(ItemDataSO itemData)
    {
        transform.SetAsLastSibling();
        canvasGroup.alpha = 1;
        itemID = itemData.ItemID;
        itemName = itemData.ItemName;
        description = itemData.Description;
        itemType = itemData.ItemType;
        itemRarity = itemData.ItemRarity;
        prices = itemData.Prices;
        itemNameText.text = itemName;
        descriptionText.text = description;
        tpyeText.text = itemType.ToString();
        pricesText.text = prices.ToString();
        RarityText.text = itemRarity.ToString();
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
