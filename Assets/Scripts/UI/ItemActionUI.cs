
using UnityEngine;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    private enum ActionType
    {
        Use,
        Equip,
        Drop,
        Close,
    }
    public RectTransform panelTransform;
    public Button useButton;
    public Button closeButton;
    public Button equipButton;
    public Button dropButton;
    public CanvasGroup canvas;
    private ItemDataSO item;
    public Vector3 movePosition;
    public int index;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts=false;
        closeButton.onClick.AddListener(() => RequestAction(ActionType.Close));
        equipButton.onClick.AddListener(() => RequestAction(ActionType.Equip));
        dropButton.onClick.AddListener(() => RequestAction(ActionType.Drop));
        useButton.onClick.AddListener(() => RequestAction(ActionType.Use));
    }
    private void OnEnable()
    {
        GameEvent.InventoryClicked += ShowPanel;
        GameEvent.BagClose += Close;
    }
    private void OnDisable()
    {
        GameEvent.InventoryClicked -= ShowPanel;
        GameEvent.BagClose -= Close;

    }
    //传入物品数据和位置，显示面板
    private void ShowPanel(ItemDataSO itemData,Vector3 position,int index)
    {
        this.index = index;
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
    private void RequestAction(ActionType actionType)
    {
        if (item == null) return;

        switch (actionType)
        {
            case ActionType.Use:
                GameEvent.TriggerItemUsed(item, index);
                Close();
                break;
            case ActionType.Equip:
                GameEvent.TriggerItemEquipped(item);
                Close();
                break;
            case ActionType.Drop:
                GameEvent.TriggerItemDropped(item);
                Close();
                break;
            case ActionType.Close:
                Close();
                break;
        }
    }
    public void Close()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
}
