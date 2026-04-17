using UnityEngine;
/// <summary>
/// 管理背包整体面板UI
/// </summary>
public class InventoryPanelUI : BasePanel
{
    [SerializeField] private InventorySlotUI[] slotUIs;
    [SerializeField] private InventorySortPanelUI sortPanelUI;

    private void OnEnable()
    {
        GameEvent.InventoryChanged += RefreshAllSlots;
        RefreshAllSlots();
        if (sortPanelUI != null)
        {
            sortPanelUI.ApplyCurrentSort();
        }
    }
    private void OnDisable()
    {
        GameEvent.InventoryChanged -= RefreshAllSlots;
        UIManager.Instance.UnregisterPanel(PanelType.bagPanel);
    }
    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.bagPanel, this);
    }
    /// <summary>
    /// 刷新所有背包格子UI
    /// </summary>
    private void RefreshAllSlots()
    {
        //如果背包格子UI为空或者背包管理器为空，则不刷新
        if (slotUIs == null || InventoryManager.Instance == null)
        {
            return;
        }
        //遍历所有背包格子UI（用格子在父节点下的 sibling 下标对应背包数据下标，避免 Inspector 数组顺序与网格子物体顺序不一致）
        for (int i = 0; i < slotUIs.Length; i++)
        {
            //如果背包格子UI为空，则不刷新
            if (slotUIs[i] == null)
            {
                continue;
            }
            int dataIndex = slotUIs[i].transform.GetSiblingIndex();
            InventorySlot slotData = dataIndex >= 0 && dataIndex < InventoryManager.Instance.slots.Count
                ? InventoryManager.Instance.slots[dataIndex]
                : null;
            slotUIs[i].UpdateSlot(slotData);
        }
    }
}
