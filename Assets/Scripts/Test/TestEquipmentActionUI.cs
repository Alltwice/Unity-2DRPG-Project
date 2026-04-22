using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 装备槽二级面板：仅负责卸下/关闭操作，显隐仅影响二级面板本身。
/// </summary>
public class TestEquipmentActionUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button closeButton;

    private TestEquipmentManager equipmentManager;
    private TestEquipmentSlot currentSlot;
    private ItemInstance currentItem;

    private void Awake()
    {
        ResolvePanelRoot();
        if (canvas == null && panelRoot != null)
            canvas = panelRoot.GetComponent<CanvasGroup>();

        if (unequipButton != null)
            unequipButton.onClick.AddListener(UnequipCurrent);
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        Close();
    }

    private void OnEnable()
    {
        GameEvent.BagClose += Close;
    }

    private void OnDisable()
    {
        GameEvent.BagClose -= Close;
    }

    public void Show(TestEquipmentSlot slot, ItemInstance item, Vector3 screenPosition, TestEquipmentManager manager)
    {
        if (item == null || item.definition == null || manager == null)
            return;

        currentSlot = slot;
        currentItem = item;
        equipmentManager = manager;

        GameObject root = GetPanelRoot();
        if (root != null)
            root.SetActive(true);

        RectTransform targetRect = panelTransform != null ? panelTransform : root != null ? root.transform as RectTransform : null;
        if (targetRect != null)
            targetRect.position = screenPosition;

        if (canvas != null)
        {
            canvas.alpha = 1f;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        currentItem = null;
        equipmentManager = null;

        if (canvas != null)
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        GameObject root = GetPanelRoot();
        if (root != null)
            root.SetActive(false);
    }

    private void UnequipCurrent()
    {
        if (equipmentManager == null || currentItem == null)
        {
            Close();
            return;
        }

        ItemInstance removed = equipmentManager.Unequip(currentSlot);
        if (removed == null)
        {
            Close();
            return;
        }

        bool addOk = TryAddItemInstanceToInventory(removed);
        if (!addOk)
        {
            equipmentManager.Equip(currentSlot, removed);
            Close();
            return;
        }

        TestPlayerStatModifiers modifiers = equipmentManager.GetComponent<TestPlayerStatModifiers>();
        if (modifiers != null && removed.definition is TestEquipmentItemSO equipmentSo)
            equipmentSo.RemoveBonuses(modifiers);

        PlayerHealth health = equipmentManager.GetComponent<PlayerHealth>();
        if (health != null)
            health.RecalculateAndClamp();

        GameEvent.TriggerInventoryChanged();
        Close();
    }

    private static bool TryAddItemInstanceToInventory(ItemInstance instance)
    {
        if (instance == null || instance.definition == null || InventoryManager.Instance == null)
            return false;

        InventoryManager inv = InventoryManager.Instance;
        inv.EnsureSlotCapacity();

        if (instance.IsStackable)
        {
            for (int i = 0; i < inv.slots.Count; i++)
            {
                InventorySlot slot = inv.slots[i];
                if (slot == null || slot.instance == null)
                    continue;

                if (slot.instance.definition == instance.definition &&
                    slot.instance.rarity == instance.rarity &&
                    slot.amount < instance.MaxStack)
                {
                    slot.amount += 1;
                    return true;
                }
            }
        }

        for (int i = 0; i < inv.slots.Count; i++)
        {
            InventorySlot slot = inv.slots[i];
            if (slot == null)
                continue;

            if (slot.instance == null && slot.amount == 0)
            {
                slot.SetInstance(1, instance);
                return true;
            }
        }

        return false;
    }

    private void ResolvePanelRoot()
    {
        if (panelTransform != null)
        {
            panelRoot = panelTransform.gameObject;
            return;
        }

        if (panelRoot != null && panelRoot.GetComponent<TestEquipmentPanelUI>() != null)
        {
            GameObject inferred = InferPanelRootFromButtons();
            if (inferred != null)
            {
                panelRoot = inferred;
                return;
            }
        }

        if (panelRoot != null)
            return;

        if (panelTransform != null)
            panelRoot = panelTransform.gameObject;
        else
            panelRoot = gameObject;
    }

    private GameObject GetPanelRoot()
    {
        ResolvePanelRoot();
        return panelRoot;
    }

    private GameObject InferPanelRootFromButtons()
    {
        if (unequipButton == null && closeButton == null)
            return null;

        Transform a = unequipButton != null ? unequipButton.transform : null;
        Transform b = closeButton != null ? closeButton.transform : null;
        if (a == null || b == null)
        {
            Transform single = a ?? b;
            return single != null && single.parent != null ? single.parent.gameObject : null;
        }

        Transform common = FindLowestCommonAncestor(a, b);
        if (common == null)
            return null;
        return common.gameObject;
    }

    private static Transform FindLowestCommonAncestor(Transform a, Transform b)
    {
        Transform cursor = a;
        while (cursor != null)
        {
            Transform probe = b;
            while (probe != null)
            {
                if (probe == cursor)
                    return cursor;
                probe = probe.parent;
            }
            cursor = cursor.parent;
        }
        return null;
    }
}
