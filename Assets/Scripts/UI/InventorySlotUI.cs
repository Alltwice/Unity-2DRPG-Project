using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 背包格子
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI amount;
    private ItemDataSO slotData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotData != null)
        {
            TipsUIPanel.Instance.ShowUIInfo(slotData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TipsUIPanel.Instance.Hide();
    }

    /// <summary>
    /// 更新背包格子UI
    /// </summary>
    public void UpdateSlot(InventorySlot slotData)
    {
        if (slotData == null)
        {
            this.slotData = null;
            icon.sprite = null;
            icon.enabled = false;
            amount.text = "";
            amount.enabled = false;
            return;
        }
        this.slotData = slotData.item;
        if (slotData.item != null && slotData.amount > 0)
        {
            icon.sprite = slotData.item.Icon;
            icon.enabled = true;
            //如果物品数量大于1，则显示物品数量
            if (slotData.amount > 1)
            {
                amount.text = slotData.amount.ToString();
                amount.enabled = true;
            }
            //如果物品数量为1，则不显示物品数量
            else
            {
                amount.text = "";
                amount.enabled = false;
            }
        }
        //如果物品为空，则清空背包格子UI
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            amount.text = "";
            amount.enabled = false;
        }
    }
}
