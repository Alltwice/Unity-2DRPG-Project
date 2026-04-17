using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 标签子面板：
/// - 负责展示/隐藏
/// - 提供“全部/武器/装备/消耗品/材料”选择
/// </summary>
public class InventoryTagSubPanelUI : MonoBehaviour
{
    [Header("标签按钮")]
    [SerializeField] private Button allButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button equipmentButton;
    [SerializeField] private Button consumableButton;
    [SerializeField] private Button materialButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private InventorySortPanelUI owner;
    private bool isVisible;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (allButton != null) allButton.onClick.AddListener(SelectAll);
        if (weaponButton != null) weaponButton.onClick.AddListener(SelectWeapon);
        if (equipmentButton != null) equipmentButton.onClick.AddListener(SelectEquipment);
        if (consumableButton != null) consumableButton.onClick.AddListener(SelectConsumable);
        if (materialButton != null) materialButton.onClick.AddListener(SelectMaterial);
    }

    private void OnDestroy()
    {
        if (allButton != null) allButton.onClick.RemoveListener(SelectAll);
        if (weaponButton != null) weaponButton.onClick.RemoveListener(SelectWeapon);
        if (equipmentButton != null) equipmentButton.onClick.RemoveListener(SelectEquipment);
        if (consumableButton != null) consumableButton.onClick.RemoveListener(SelectConsumable);
        if (materialButton != null) materialButton.onClick.RemoveListener(SelectMaterial);
    }

    public void BindOwner(InventorySortPanelUI panel)
    {
        owner = panel;
    }

    public void Toggle()
    {
        if (isVisible)
            HideImmediate();
        else
            ShowImmediate();
    }

    public void HideImmediate()
    {
        isVisible = false;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowImmediate()
    {
        isVisible = true;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void SelectAll()
    {
        if (owner != null)
            owner.SetTagAndApply(null);
        HideImmediate();
    }

    public void SelectWeapon()
    {
        if (owner != null)
            owner.SetTagAndApply(ItemType.武器);
        HideImmediate();
    }

    public void SelectEquipment()
    {
        if (owner != null)
            owner.SetTagAndApply(ItemType.装备);
        HideImmediate();
    }

    public void SelectConsumable()
    {
        if (owner != null)
            owner.SetTagAndApply(ItemType.消耗品);
        HideImmediate();
    }

    public void SelectMaterial()
    {
        if (owner != null)
            owner.SetTagAndApply(ItemType.材料);
        HideImmediate();
    }
}

