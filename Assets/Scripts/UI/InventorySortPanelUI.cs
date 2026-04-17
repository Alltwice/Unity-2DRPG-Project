using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包排序主面板：
/// - 价格按钮：重复点击同按钮时切换正/倒序
/// - 品质按钮：重复点击同按钮时切换正/倒序
/// - 标签按钮：打开/关闭标签子面板
/// </summary>
public class InventorySortPanelUI : MonoBehaviour
{
    [Header("主按钮")]
    [SerializeField] private Button priceSortButton;
    [SerializeField] private Button raritySortButton;
    [SerializeField] private Button tagSortButton;

    [Header("标签子面板")]
    [SerializeField] private InventoryTagSubPanelUI tagSubPanel;

    [Header("默认状态")]
    [SerializeField] private InventorySortMode currentMode = InventorySortMode.ByPrice;
    [SerializeField] private InventorySortDirection priceDirection = InventorySortDirection.Ascending;
    [SerializeField] private InventorySortDirection rarityDirection = InventorySortDirection.Ascending;
    [SerializeField] private ItemType defaultTagType = ItemType.武器;
    [SerializeField] private bool useDefaultTag = false;

    private ItemType? selectedTag;

    private void Awake()
    {
        selectedTag = useDefaultTag ? defaultTagType : (ItemType?)null;

        if (priceSortButton != null)
            priceSortButton.onClick.AddListener(OnPriceButtonClicked);

        if (raritySortButton != null)
            raritySortButton.onClick.AddListener(OnRarityButtonClicked);

        if (tagSortButton != null)
            tagSortButton.onClick.AddListener(ToggleTagPanel);

        if (tagSubPanel != null)
        {
            tagSubPanel.BindOwner(this);
            tagSubPanel.HideImmediate();
        }
    }

    private void OnDestroy()
    {
        if (priceSortButton != null)
            priceSortButton.onClick.RemoveListener(OnPriceButtonClicked);
        if (raritySortButton != null)
            raritySortButton.onClick.RemoveListener(OnRarityButtonClicked);
        if (tagSortButton != null)
            tagSortButton.onClick.RemoveListener(ToggleTagPanel);
    }

    public void OnPriceButtonClicked()
    {
        if (currentMode == InventorySortMode.ByPrice)
            priceDirection = ToggleDirection(priceDirection);

        currentMode = InventorySortMode.ByPrice;
        ApplyCurrentSort();
    }

    public void OnRarityButtonClicked()
    {
        if (currentMode == InventorySortMode.ByRarity)
            rarityDirection = ToggleDirection(rarityDirection);

        currentMode = InventorySortMode.ByRarity;
        ApplyCurrentSort();
    }

    public void ToggleTagPanel()
    {
        if (tagSubPanel == null)
            return;

        tagSubPanel.Toggle();
    }
    //设置标签并传入具体标签，由子面板调用
    public void SetTagAndApply(ItemType? tagType)
    {
        selectedTag = tagType;
        currentMode = InventorySortMode.ByTag;
        ApplyCurrentSort();
    }
    //记录当前状态调用排序
    public void ApplyCurrentSort()
    {
        InventorySortDirection direction = currentMode == InventorySortMode.ByRarity
            ? rarityDirection
            : priceDirection;

        ItemType? tagArg = currentMode == InventorySortMode.ByTag
            ? selectedTag
            : (ItemType?)null;

        InventorySortFilterController.Apply(tagArg, currentMode, direction);
    }
    //改变方向
    private static InventorySortDirection ToggleDirection(InventorySortDirection direction)
    {
        return direction == InventorySortDirection.Ascending
            ? InventorySortDirection.Descending
            : InventorySortDirection.Ascending;
    }
}

