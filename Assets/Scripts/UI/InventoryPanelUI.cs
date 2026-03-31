using UnityEngine;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slotUIs;

    private void OnEnable()
    {
        GameEvent.InventoryChanged += RefreshAllSlots;
        RefreshAllSlots();
    }

    private void OnDisable()
    {
        GameEvent.InventoryChanged -= RefreshAllSlots;
    }

    public void RefreshAllSlots()
    {
        if (slotUIs == null || InventoryManager.Instance == null)
        {
            return;
        }

        for (int i = 0; i < slotUIs.Length; i++)
        {
            if (slotUIs[i] == null)
            {
                continue;
            }

            InventorySlot slotData = i < InventoryManager.Instance.slots.Count
                ? InventoryManager.Instance.slots[i]
                : null;

            slotUIs[i].UpdateSlot(slotData);
        }
    }
}
