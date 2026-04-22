using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 固定装备槽主面板：负责槽位展示刷新，不处理二级操作逻辑。
/// </summary>
public class TestEquipmentPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class SlotView
    {
        public TestEquipmentSlot slotType;
        public Image slotBackground;
        public Image iconImage;
        public Text nameText;
        public Text rarityText;
        public TestEquipmentSlotUI clickTarget;
    }

    [SerializeField] private SlotView[] slotViews;
    [SerializeField] private TestEquipmentManager equipmentManager;
    [SerializeField] private TestEquipmentActionUI equipmentActionUI;
    [SerializeField] private CanvasGroup canvas;

    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponent<CanvasGroup>();
        BindSlotDependencies();
    }

    private void OnEnable()
    {
        GameEvent.InventoryChanged += Refresh;
        GameEvent.BagClose += Close;
    }

    private void OnDisable()
    {
        GameEvent.InventoryChanged -= Refresh;
        GameEvent.BagClose -= Close;
    }

    public void Refresh()
    {
        ResolveEquipmentManager();
        if (equipmentManager == null || slotViews == null)
            return;
        BindSlotDependencies();

        int equippedCount = 0;
        int iconEnabledCount = 0;
        for (int i = 0; i < slotViews.Length; i++)
        {
            SlotView view = slotViews[i];
            if (view == null)
                continue;

            if (view.slotBackground != null)
                view.slotBackground.enabled = true;

            ItemInstance instance = equipmentManager.GetSlot(view.slotType);
            bool hasItem = instance != null && instance.definition != null;
            if (hasItem)
                equippedCount++;

            if (view.iconImage != null)
            {
                view.iconImage.sprite = hasItem ? instance.Icon : null;
                view.iconImage.enabled = hasItem && instance.Icon != null;
                if (view.iconImage.enabled)
                    iconEnabledCount++;
            }

            if (view.nameText != null)
                view.nameText.text = hasItem ? instance.ItemName : string.Empty;

            if (view.rarityText != null)
                view.rarityText.text = hasItem ? instance.rarity.ToString() : string.Empty;
        }
    }

    public void Open()
    {
        if (canvas != null)
        {
            canvas.alpha = 1f;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }
        else
        {
            gameObject.SetActive(true);
        }

        ResolveEquipmentManager();
        BindSlotDependencies();
        Refresh();
    }

    public void Close()
    {
        if (equipmentActionUI != null)
            equipmentActionUI.Close();

        if (canvas != null)
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void BindSlotDependencies()
    {
        ResolveEquipmentManager();
        if (slotViews == null)
            return;

        for (int i = 0; i < slotViews.Length; i++)
        {
            SlotView view = slotViews[i];
            if (view == null || view.clickTarget == null)
                continue;

            view.clickTarget.Bind(view.slotType, equipmentManager, equipmentActionUI);
        }
    }

    private void ResolveEquipmentManager()
    {
        TestEquipmentManager resolved = null;

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
            resolved = player.GetComponent<TestEquipmentManager>();

        if (resolved == null)
            resolved = Object.FindFirstObjectByType<TestEquipmentManager>();

        if (resolved != null && equipmentManager != resolved)
        {
            equipmentManager = resolved;
        }
    }
}
