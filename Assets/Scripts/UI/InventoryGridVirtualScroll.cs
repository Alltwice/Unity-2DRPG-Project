using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包网格虚拟滚动：仅实例化视口附近格子，数据仍来自 <see cref="InventoryManager.slots"/>。
/// Content 建议：anchor/pivot 左上角 (0,1)，由本脚本设置 <see cref="RectTransform.sizeDelta"/>。
/// </summary>
public class InventoryGridVirtualScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int columns = 5;
    [SerializeField] private Vector2 cellSize = new Vector2(64f, 64f);
    [SerializeField] private Vector2 spacing;
    [SerializeField] private float paddingLeft;
    [SerializeField] private float paddingRight;
    [SerializeField] private float paddingTop;
    [SerializeField] private float paddingBottom;
    [SerializeField] private int bufferRows = 2;

    private InventorySlotUIPool _pool;
    private RectTransform _content;
    private RectTransform _viewport;
    private bool _eventSubscribed;
    private readonly Dictionary<int, InventorySlotUI> _activeByDataIndex = new Dictionary<int, InventorySlotUI>(64);
    private readonly List<int> _indicesScratch = new List<int>(32);

    private void Awake()
    {
        _pool = GetComponent<InventorySlotUIPool>();
        if (scrollRect != null)
        {
            _content = scrollRect.content;
            _viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.transform as RectTransform;
        }
    }

    private void OnEnable()
    {
        if (scrollRect != null && _content == null)
        {
            _content = scrollRect.content;
            _viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.transform as RectTransform;
        }

        SubscribeEvents();
        if (_content != null && _viewport != null)
            RefreshLayout();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
        ReleaseAllCellsToPool();
    }

    //外部主动注册
    public void BindRuntimeReferences(
        ScrollRect sr,
        RectTransform content,
        RectTransform viewport,
        InventorySlotUIPool pool,
        int cols,
        Vector2 cell,
        Vector2 space,
        RectOffset pad)
    {
        scrollRect = sr;
        _content = content;
        _viewport = viewport;
        _pool = pool != null ? pool : GetComponent<InventorySlotUIPool>();
        columns = Mathf.Max(1, cols);
        cellSize = cell;
        spacing = space;
        paddingLeft = pad.left;
        paddingRight = pad.right;
        paddingTop = pad.top;
        paddingBottom = pad.bottom;
        SubscribeEvents();
        if (isActiveAndEnabled && _content != null && _viewport != null)
            RefreshLayout();
    }

    private void SubscribeEvents()
    {
        if (_eventSubscribed || scrollRect == null || _content == null || _viewport == null)
            return;
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
        GameEvent.InventoryChanged += OnInventoryChanged;
        _eventSubscribed = true;
    }

    private void UnsubscribeEvents()
    {
        if (!_eventSubscribed)
            return;
        if (scrollRect != null)
            scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        GameEvent.InventoryChanged -= OnInventoryChanged;
        _eventSubscribed = false;
    }

    private void OnScrollChanged(Vector2 _) => RefreshVisibleCells();

    private void OnInventoryChanged() => RefreshLayout();

    //计算显示，缓冲区域
    public void RefreshLayout()
    {
        if (_content == null || InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.EnsureSlotCapacity();
        int slotCount = InventoryManager.Instance.slots.Count;
        int rows = slotCount <= 0 ? 0 : Mathf.CeilToInt((float)slotCount / columns);

        float stepX = cellSize.x + spacing.x;
        float stepY = cellSize.y + spacing.y;
        float contentW = paddingLeft + paddingRight + columns * cellSize.x + Mathf.Max(0, columns - 1) * spacing.x;
        float contentH = paddingTop + paddingBottom + rows * cellSize.y + Mathf.Max(0, rows - 1) * spacing.y;

        if (rows == 0)
            contentH = paddingTop + paddingBottom;

        _content.anchorMin = new Vector2(0f, 1f);
        _content.anchorMax = new Vector2(0f, 1f);
        _content.pivot = new Vector2(0f, 1f);
        _content.sizeDelta = new Vector2(contentW, contentH);

        RefreshVisibleCells();
    }
    //刷新UI同时设定每个格子位置
    private void RefreshVisibleCells()
    {
        if (_pool == null || _content == null || _viewport == null || InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.EnsureSlotCapacity();
        List<InventorySlot> slots = InventoryManager.Instance.slots;
        int slotCount = slots.Count;
        if (columns <= 0)
            return;

        if (slotCount <= 0)
        {
            ReleaseAllCellsToPool();
            return;
        }

        int rows = Mathf.CeilToInt((float)slotCount / columns);
        float stepX = cellSize.x + spacing.x;
        float stepY = cellSize.y + spacing.y;

        if (!TryGetVisibleRowRange(rows, stepY, out int minRow, out int maxRow))
        {
            minRow = 0;
            maxRow = rows - 1;
        }

        minRow = Mathf.Clamp(minRow - bufferRows, 0, Mathf.Max(0, rows - 1));
        maxRow = Mathf.Clamp(maxRow + bufferRows, 0, Mathf.Max(0, rows - 1));

        int startIndex = minRow * columns;
        int endExclusive = Mathf.Min(slotCount, (maxRow + 1) * columns);

        _indicesScratch.Clear();
        foreach (KeyValuePair<int, InventorySlotUI> kv in _activeByDataIndex)
        {
            if (kv.Key < startIndex || kv.Key >= endExclusive || kv.Key >= slotCount)
                _indicesScratch.Add(kv.Key);
        }

        for (int k = 0; k < _indicesScratch.Count; k++)
        {
            int idx = _indicesScratch[k];
            if (_activeByDataIndex.TryGetValue(idx, out InventorySlotUI released))
            {
                _pool.Return(released);
                _activeByDataIndex.Remove(idx);
            }
        }

        for (int i = startIndex; i < endExclusive; i++)
        {
            if (!_activeByDataIndex.TryGetValue(i, out InventorySlotUI cell) || cell == null)
            {
                cell = _pool.Get();
                if (cell == null)
                    continue;
                _activeByDataIndex[i] = cell;
            }

            int row = i / columns;
            int col = i % columns;

            RectTransform rt = cell.transform as RectTransform;
            if (rt != null)
            {
                rt.SetParent(_content, false);
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 1f);
                rt.sizeDelta = cellSize;
                rt.anchoredPosition = new Vector2(
                    paddingLeft + col * stepX,
                    -paddingTop - row * stepY);
            }

            cell.SetBoundDataIndex(i);
            cell.UpdateSlot(slots[i]);
        }
    }
    //获取可见部分范围
    private bool TryGetVisibleRowRange(int rows, float stepY, out int minRow, out int maxRow)
    {
        minRow = int.MaxValue;
        maxRow = int.MinValue;

        Vector3[] corners = new Vector3[4];
        _viewport.GetWorldCorners(corners);
        float viewMinY = float.MaxValue;
        float viewMaxY = float.MinValue;
        for (int i = 0; i < 4; i++)
        {
            Vector3 local = _content.InverseTransformPoint(corners[i]);
            viewMinY = Mathf.Min(viewMinY, local.y);
            viewMaxY = Mathf.Max(viewMaxY, local.y);
        }

        for (int r = 0; r < rows; r++)
        {
            float rowTop = -paddingTop - r * stepY;
            float rowBottom = rowTop - cellSize.y;
            if (rowBottom < viewMaxY && rowTop > viewMinY)
            {
                if (r < minRow) minRow = r;
                if (r > maxRow) maxRow = r;
            }
        }

        if (minRow == int.MaxValue)
            return false;

        return true;
    }

    private void ReleaseAllCellsToPool()
    {
        if (_pool == null)
        {
            _activeByDataIndex.Clear();
            return;
        }

        foreach (KeyValuePair<int, InventorySlotUI> kv in _activeByDataIndex)
        {
            if (kv.Value != null)
                _pool.Return(kv.Value);
        }

        _activeByDataIndex.Clear();
    }
}
