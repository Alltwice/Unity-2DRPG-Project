using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// 背包格子
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler
{
    [System.Serializable]
    private struct RarityColorEntry
    {
        public ItemRarity rarity;
        public Color color;
    }

    [Header("稀有度底图")]
    [SerializeField] private Image rarityBackground;
    [SerializeField] private Color emptySlotColor = Color.clear;
    [SerializeField] private List<RarityColorEntry> rarityColors = new List<RarityColorEntry>
    {
        new RarityColorEntry { rarity = ItemRarity.普通, color = new Color(0.70f, 0.70f, 0.70f, 0.30f) },
        new RarityColorEntry { rarity = ItemRarity.稀有, color = new Color(0.30f, 0.56f, 1.00f, 0.35f) },
        new RarityColorEntry { rarity = ItemRarity.史诗, color = new Color(0.73f, 0.33f, 1.00f, 0.35f) },
        new RarityColorEntry { rarity = ItemRarity.传奇, color = new Color(1.00f, 0.62f, 0.20f, 0.40f) },
    };

    public Image icon;
    public TextMeshProUGUI amount;
    private ItemInstance slotData;
    private int amountInt;
    private Coroutine delayShow;
    private CanvasGroup iconCanvas;
    private Transform originalTransform;
    private Vector2 originalVector2;
    private Transform originalAmountTransform;
    private Vector2 originalAmountVector2;
    private Vector2 amountDragOffset;
    private Transform dragRoot;
    private bool isDragging;
    private readonly Dictionary<ItemRarity, Color> rarityColorMap = new Dictionary<ItemRarity, Color>();

    /// <summary>与 <see cref="InventoryManager.slots"/> 对齐的逻辑下标；虚拟滚动下不等于 Transform 兄弟顺序。</summary>
    public int BoundDataIndex { get; private set; }

    public void SetBoundDataIndex(int index)
    {
        BoundDataIndex = index;
    }

    private void Awake()
    {
        CacheRarityColorMap();
        iconCanvas = icon != null ? icon.GetComponent<CanvasGroup>() : null;
        if (iconCanvas == null && icon != null)
        {
            iconCanvas = icon.gameObject.AddComponent<CanvasGroup>();
        }
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        dragRoot = parentCanvas != null ? parentCanvas.rootCanvas.transform : null;
        if (amount != null)
        {
            amount.raycastTarget = false;
        }
        if (rarityBackground != null)
        {
            rarityBackground.raycastTarget = false;
            rarityBackground.color = emptySlotColor;
            rarityBackground.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delayShow=StartCoroutine(ShowInfo(0.5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TipsUIPanel.Instance.Hide();
        if (delayShow != null)
            StopCoroutine(delayShow);
    }
    IEnumerator ShowInfo(float watiTime)
    {
        yield return new WaitForSeconds(watiTime);
        if (slotData != null)
        {
            TipsUIPanel.Instance.ShowUIInfo(slotData,amountInt);
        }
    }

    /// <summary>
    /// 更新背包格子UI
    /// </summary>
    public void UpdateSlot(InventorySlot slotData)
    {
        if (slotData == null)
        {
            this.slotData = null;
            amountInt = 0;
            UpdateRarityBackground(null);
            if (icon != null)
            {
                icon.sprite = null;
                icon.enabled = false;
            }

            if (amount != null)
            {
                amount.text = "";
                amount.enabled = false;
            }

            return;
        }

        amountInt = slotData.amount;
        this.slotData = slotData.instance;
        if (slotData.instance != null && slotData.amount > 0)
        {
            UpdateRarityBackground(slotData.instance);
            if (icon != null)
            {
                icon.sprite = slotData.instance.Icon;
                icon.enabled = true;
            }

            //如果物品数量大于1，则显示物品数量
            if (amount != null)
            {
                if (slotData.amount > 1)
                {
                    amount.text = slotData.amount.ToString();
                    amount.enabled = true;
                }
                else
                {
                    amount.text = "";
                    amount.enabled = false;
                }
            }
        }
        //如果物品为空，则清空背包格子UI
        else
        {
            UpdateRarityBackground(null);
            if (icon != null)
            {
                icon.sprite = null;
                icon.enabled = false;
            }

            if (amount != null)
            {
                amount.text = "";
                amount.enabled = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //事件驱动，点击后不为空，且为左键点击就触发点击事件
        if(slotData!=null&& eventData.button == PointerEventData.InputButton.Left)
        {
            GameEvent.TriggerInventoryClicked(slotData, transform.position, BoundDataIndex);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(slotData==null)
        {
            return;
        }
        isDragging = true;
        //拖入时将图标移到最上层，记录原始位置和父对象，方便拖动结束后还原
        originalTransform = icon.transform.parent;
        originalVector2=icon.transform.localPosition;
        if (dragRoot != null)
        {
            icon.transform.SetParent(dragRoot);
            icon.transform.SetAsLastSibling();
            if (amount != null)
            {
                originalAmountTransform = amount.transform.parent;
                originalAmountVector2 = amount.rectTransform.localPosition;
                if (amount.enabled)
                {
                    amountDragOffset = amount.rectTransform.position - icon.rectTransform.position;
                    amount.transform.SetParent(dragRoot);
                    amount.transform.SetAsLastSibling();
                }
                else
                {
                    amountDragOffset = Vector2.zero;
                }
            }
        }
        //防止遮挡视线让下面无法触发后续逻辑
        if (iconCanvas != null)
        {
            iconCanvas.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            return;
        }
        isDragging = false;
        //结束后就送回原来的层级和位置，恢复射线检测
        if (originalTransform != null)
        {
            icon.transform.SetParent(originalTransform);
        }
        icon.transform.localPosition = originalVector2;
        if (amount != null && originalAmountTransform != null)
        {
            amount.transform.SetParent(originalAmountTransform);
            amount.rectTransform.localPosition = originalAmountVector2;
        }
        if (iconCanvas != null)
        {
            iconCanvas.blocksRaycasts = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            return;
        }
        //抓取的时候跟着鼠标走
        Vector2 mousePos = Mouse.current.position.ReadValue();
        icon.rectTransform.position = mousePos;
        if (amount != null && amount.enabled)
        {
            amount.rectTransform.position = mousePos + amountDragOffset;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI fromSlotUI = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<InventorySlotUI>() : null;
        int fromIndex = fromSlotUI != null ? fromSlotUI.BoundDataIndex : -1;
        int toIndex = BoundDataIndex;
        bool fromDataEmpty = true;
        if (fromIndex >= 0 && fromIndex < InventoryManager.Instance.slots.Count)
        {
            InventorySlot fromDataSlot = InventoryManager.Instance.slots[fromIndex];
            fromDataEmpty = fromDataSlot == null || fromDataSlot.instance == null || fromDataSlot.amount <= 0;
        }
        // 如果拖拽源有效、不是自己，且源槽位确实有数据才允许交换
        if (fromSlotUI != null && fromSlotUI != this && !fromDataEmpty)
        {
            // 获取拖拽源的下标，和我自己的下标
            InventoryManager.Instance.SwapItems(fromIndex, toIndex);
        }
    }

    private void CacheRarityColorMap()
    {
        rarityColorMap.Clear();
        for (int i = 0; i < rarityColors.Count; i++)
        {
            rarityColorMap[rarityColors[i].rarity] = rarityColors[i].color;
        }
    }

    private void UpdateRarityBackground(ItemInstance itemInstance)
    {
        if (rarityBackground == null)
            return;

        if (itemInstance == null)
        {
            rarityBackground.color = emptySlotColor;
            rarityBackground.enabled = false;
            return;
        }

        if (!rarityColorMap.TryGetValue(itemInstance.rarity, out Color rarityColor))
        {
            rarityColor = Color.white;
        }

        rarityBackground.color = rarityColor;
        rarityBackground.enabled = true;
    }
}
