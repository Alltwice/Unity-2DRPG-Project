
using UnityEngine;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    public RectTransform panelTransform;
    public Button useButton;
    public Button closeButton;
    public Button equipButton;
    public Button dropButton;
    public CanvasGroup canvas;
    private ItemDataSO item;
    public Vector3 movePosition;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts=false;
    }
    private void OnEnable()
    {
        GameEvent.InventoryClicked += ShowPanel;
    }
    private void OnDisable()
    {
        GameEvent.InventoryClicked -= ShowPanel;
    }
    //传入物品数据和位置，显示面板
    private void ShowPanel(ItemDataSO itemData,Vector3 position)
    {
        item = itemData;
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        panelTransform.position = position + movePosition;
        //通过类型判断显示按钮
        useButton.gameObject.SetActive(itemData.ItemType == ItemType.消耗品);
        equipButton.gameObject.SetActive(itemData.ItemType == ItemType.武器 || itemData.ItemType == ItemType.装备);
        dropButton.gameObject.SetActive(true); // 默认所有物品可丢弃，特殊任务物品除外
    }
}
