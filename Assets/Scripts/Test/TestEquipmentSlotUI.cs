using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 固定装备槽点击入口：有物品弹二级面板，无物品仅关闭二级面板。
/// </summary>
public class TestEquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TestEquipmentSlot slot;
    [SerializeField] private TestEquipmentManager equipmentManager;
    [SerializeField] private TestEquipmentActionUI actionUI;
    [SerializeField] private RectTransform anchorTransform;
    [SerializeField] private Vector3 panelOffset;

    private void Awake()
    {
        if (anchorTransform == null)
            anchorTransform = transform as RectTransform;
    }

    public void Bind(TestEquipmentSlot slotType, TestEquipmentManager manager, TestEquipmentActionUI panel)
    {
        slot = slotType;
        equipmentManager = manager;
        actionUI = panel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (equipmentManager == null || actionUI == null)
            return;

        ItemInstance instance = equipmentManager.GetSlot(slot);
        if (instance == null || instance.definition == null)
        {
            actionUI.Close();
            return;
        }

        Vector3 anchorPos = anchorTransform != null ? anchorTransform.position : transform.position;
        actionUI.Show(slot, instance, anchorPos + panelOffset, equipmentManager);
    }
}
