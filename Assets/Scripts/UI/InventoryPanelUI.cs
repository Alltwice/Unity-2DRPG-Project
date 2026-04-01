using UnityEngine;
/// <summary>
/// 管理背包整体面板UI
/// </summary>
public class InventoryPanelUI : BasePanel
{
    [SerializeField] private InventorySlotUI[] slotUIs;

    private void OnEnable()
    {
        GameEvent.InventoryChanged += RefreshAllSlots;
        RefreshAllSlots();
    }
    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.bagPanel, this);
    }
    private void OnDisable()
    {
        GameEvent.InventoryChanged -= RefreshAllSlots;
        UIManager.Instance.UnregisterPanel(PanelType.bagPanel);
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
        //遍历所有背包格子UI
        for (int i = 0; i < slotUIs.Length; i++)
        {
            //如果背包格子UI为空，则不刷新
            if (slotUIs[i] == null)
            {
                continue;
            }
            //如果背包格子UI索引小于背包管理器物品数量，则获取背包管理器物品，否则获取空物品
            InventorySlot slotData = i < InventoryManager.Instance.slots.Count
                ? InventoryManager.Instance.slots[i]
                : null;
            //遍历并更新背包格子UI
            slotUIs[i].UpdateSlot(slotData);
        }
    }
}
