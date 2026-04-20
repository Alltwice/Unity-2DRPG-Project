using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包格子 UI 对象池；供虚拟滚动复用 <see cref="InventorySlotUI"/> 实例。
/// </summary>
public class InventorySlotUIPool : MonoBehaviour
{
    [SerializeField] private InventorySlotUI prefab;
    [SerializeField] private InventorySlotUI cloneSource;
    [SerializeField] private RectTransform parent;
    [SerializeField] private int prewarmCount = 24;
    [SerializeField] private Vector2 cloneCellSize = new Vector2(50f, 50f);

    private readonly List<InventorySlotUI> _instances = new List<InventorySlotUI>();

    public RectTransform Parent => parent != null ? parent : transform as RectTransform;

    /// <summary>运行时绑定：父节点为 Content，从场景格子克隆。</summary>
    public void Configure(RectTransform parentRt, InventorySlotUI sourceForClone, Vector2 cellSize)
    {
        parent = parentRt;
        prefab = null;
        cloneSource = sourceForClone;
        if (cellSize.x > 0f && cellSize.y > 0f)
            cloneCellSize = cellSize;
        Prewarm();
    }

    private void Awake()
    {
        if (parent == null)
            parent = transform as RectTransform;
    }

    private void Start()
    {
        if (_instances.Count == 0 && (prefab != null || cloneSource != null) && parent != null)
            Prewarm();
    }

    private void Prewarm()
    {
        if (parent == null)
            return;
        if (prefab == null && cloneSource == null)
            return;
        while (_instances.Count < prewarmCount)
            CreateInstance(active: false);
    }

    private InventorySlotUI CreateInstance(bool active)
    {
        InventorySlotUI ui;
        if (prefab != null)
        {
            ui = Instantiate(prefab, parent);
        }
        else if (cloneSource != null)
        {
            ui = Instantiate(cloneSource.gameObject, parent).GetComponent<InventorySlotUI>();
            RectTransform rt = ui.transform as RectTransform;
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.sizeDelta = cloneCellSize;
            }
        }
        else
        {
            return null;
        }

        ui.gameObject.SetActive(active);
        _instances.Add(ui);
        return ui;
    }

    /// <summary>从池中取一个可用实例（必要时扩容）。</summary>
    public InventorySlotUI Get()
    {
        if (parent == null)
            return null;
        if (prefab == null && cloneSource == null)
            return null;

        for (int i = 0; i < _instances.Count; i++)
        {
            if (_instances[i] != null && !_instances[i].gameObject.activeSelf)
            {
                _instances[i].gameObject.SetActive(true);
                return _instances[i];
            }
        }

        return CreateInstance(active: true);
    }

    /// <summary>回收实例到池中（隐藏并保留在父节点下）。</summary>
    public void Return(InventorySlotUI slot)
    {
        if (slot == null)
            return;
        slot.gameObject.SetActive(false);
    }

    /// <summary>隐藏并回收当前池中所有活动实例。</summary>
    public void ReleaseAllActive()
    {
        for (int i = 0; i < _instances.Count; i++)
        {
            if (_instances[i] != null && _instances[i].gameObject.activeSelf)
                _instances[i].gameObject.SetActive(false);
        }
    }
}
