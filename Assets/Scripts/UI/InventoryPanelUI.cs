using UnityEngine;

/// <summary>
/// 管理背包整体面板UI
/// </summary>
public class InventoryPanelUI : BasePanel
{
    [SerializeField] private InventoryGridVirtualScroll virtualScroll;
    [SerializeField] private InventorySortPanelUI sortPanelUI;
    [SerializeField] private TestEquipmentPanelUI equipmentPanelUI;

    /// <summary>避免 BasePanel.Awake 里首次 Close() 误发 BagClose；仅在玩家打开过背包后再关时收起二级 UI。</summary>
    private bool _wasOpenedForSession;

    protected override void Awake()
    {
        // 避免 Inspector 未设置时默认为 pausePanel(0)，导致 UIManager 等依赖 panelType 的逻辑误判
        panelType = PanelType.bagPanel;
        base.Awake();
        if (sortPanelUI == null)
            sortPanelUI = GetComponent<InventorySortPanelUI>();
        if (virtualScroll == null)
            virtualScroll = InventoryBagRuntimeVirtualizer.CreateIfNeeded(transform);
    }

    public override void Open()
    {
        base.Open();
        if (equipmentPanelUI != null)
            equipmentPanelUI.Open();
        _wasOpenedForSession = true;
    }

    public override void Close()
    {
        if (_wasOpenedForSession)
        {
            GameEvent.TriggerBagClose();
            _wasOpenedForSession = false;
        }

        if (equipmentPanelUI != null)
            equipmentPanelUI.Close();
        base.Close();
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