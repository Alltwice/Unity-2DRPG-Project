using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amount;
    public void UpdateSlot(InventorySlot slotData)
    {
        if (slotData != null && slotData.item != null && slotData.amount > 0)
        {
            icon = slotData.item.Icon;
            icon.enabled = true;
            if (slotData.amount > 1)
            {
                amount.text = slotData.amount.ToString();
                icon.enabled = true;
            }
            else
            {
                amount.enabled = false;
            }
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            amount.text = "";
            amount.enabled = false;
        }
    }
}
