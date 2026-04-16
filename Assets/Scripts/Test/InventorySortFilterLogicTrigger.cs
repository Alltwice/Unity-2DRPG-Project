using UnityEngine;

/// <summary>
/// 无 UI 验证入口（逻辑层）：
/// 通过 ContextMenu 在 Inspector 里一键触发 Apply/Reset，验证排序与标签优先效果。
/// </summary>
public class InventorySortFilterLogicTrigger : MonoBehaviour
{
    [Header("标签 Boost（BoostEnabled=true 时生效）")]
    public bool BoostEnabled = false;
    public ItemType BoostedType = ItemType.武器;

    [Header("排序")]
    public InventorySortField SortField = InventorySortField.Price;
    public InventorySortDirection SortDirection = InventorySortDirection.Ascending;

    [Header("是否启动时自动应用（用于调试）")]
    public bool ApplyOnStart = false;

    private void Start()
    {
        if (ApplyOnStart)
            ApplyNow();
    }

    [ContextMenu("Apply TagBoost + Sort")]
    public void ApplyNow()
    {
        ItemType? boostedType = BoostEnabled ? BoostedType : (ItemType?)null;

        InventorySortFilterController.Apply(
            boostedType,
            SortField,
            SortDirection);
    }

    [ContextMenu("Reset Original Inventory Order")]
    public void ResetOriginalOrder()
    {
        InventorySortFilterController.ResetOriginalOrder();
    }
}

