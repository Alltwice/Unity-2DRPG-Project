using UnityEngine;

/// <summary>
/// 管理背包整体面板UI
/// </summary>
public class InventoryPanelUI : BasePanel
{
    [SerializeField] private InventoryGridVirtualScroll virtualScroll;
    [SerializeField] private InventorySortPanelUI sortPanelUI;

    protected override void Awake()
    {
        base.Awake();
        if (sortPanelUI == null)
            sortPanelUI = GetComponent<InventorySortPanelUI>();
        if (virtualScroll == null)
            virtualScroll = InventoryBagRuntimeVirtualizer.CreateIfNeeded(transform);
    }

    private void OnEnable()
    {
        if (sortPanelUI != null)
            sortPanelUI.ApplyCurrentSort();

        if (virtualScroll != null)
            virtualScroll.RefreshLayout();
    }

    private void OnDisable()
    {
        UIManager.Instance.UnregisterPanel(PanelType.bagPanel);
    }

    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.bagPanel, this);
    }
}