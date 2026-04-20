using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 在缺少 Inspector 配置的 <see cref="InventoryGridVirtualScroll"/> 时，从 BagUI 下名为 Layout 的
/// <see cref="GridLayoutGroup"/> + 首个格子克隆出 ScrollRect、对象池与虚拟滚动（并移除其余静态格子）。
/// </summary>
public static class InventoryBagRuntimeVirtualizer
{
    public static InventoryGridVirtualScroll CreateIfNeeded(Transform bagPanelRoot)
    {
        if (bagPanelRoot == null)
            return null;

        Transform layoutTf = bagPanelRoot.Find("Layout");
        if (layoutTf == null)
            return null;

        var layout = layoutTf as RectTransform;
        if (layout == null)
            return null;

        var grid = layout.GetComponent<GridLayoutGroup>();
        if (grid == null)
            return null;

        InventorySlotUI template = null;
        for (int i = 0; i < layout.childCount; i++)
        {
            var slot = layout.GetChild(i).GetComponent<InventorySlotUI>();
            if (slot != null)
            {
                template = slot;
                break;
            }
        }

        if (template == null)
            return null;

        for (int i = layout.childCount - 1; i >= 0; i--)
        {
            Transform ch = layout.GetChild(i);
            if (ch.GetComponent<InventorySlotUI>() == null)
                continue;
            if (ch.gameObject == template.gameObject)
                continue;
            UnityEngine.Object.Destroy(ch.gameObject);
        }

        grid.enabled = false;

        GameObject scrollRoot = new GameObject("BagVirtualScroll", typeof(RectTransform));
        scrollRoot.layer = layout.gameObject.layer;
        RectTransform scrollRt = scrollRoot.GetComponent<RectTransform>();
        scrollRt.SetParent(layout, false);
        StretchToParent(scrollRt);

        ScrollRect sr = scrollRoot.AddComponent<ScrollRect>();
        sr.horizontal = false;
        sr.vertical = true;
        sr.movementType = ScrollRect.MovementType.Clamped;
        sr.scrollSensitivity = 35f;

        GameObject vpGo = new GameObject("Viewport", typeof(RectTransform));
        vpGo.layer = scrollRoot.layer;
        RectTransform vpRt = vpGo.GetComponent<RectTransform>();
        vpRt.SetParent(scrollRt, false);
        StretchToParent(vpRt);
        Image vpImg = vpGo.AddComponent<Image>();
        vpImg.color = new Color(1f, 1f, 1f, 0.02f);
        vpImg.raycastTarget = true;
        vpGo.AddComponent<Mask>().showMaskGraphic = false;

        GameObject contentGo = new GameObject("Content", typeof(RectTransform));
        contentGo.layer = scrollRoot.layer;
        RectTransform cRt = contentGo.GetComponent<RectTransform>();
        cRt.SetParent(vpRt, false);
        cRt.anchorMin = new Vector2(0f, 1f);
        cRt.anchorMax = new Vector2(0f, 1f);
        cRt.pivot = new Vector2(0f, 1f);
        cRt.anchoredPosition = Vector2.zero;
        cRt.sizeDelta = Vector2.zero;

        sr.viewport = vpRt;
        sr.content = cRt;

        template.gameObject.SetActive(false);
        template.transform.SetParent(cRt, false);

        var pool = scrollRoot.AddComponent<InventorySlotUIPool>();
        pool.Configure(cRt, template, grid.cellSize);

        var vs = scrollRoot.AddComponent<InventoryGridVirtualScroll>();
        vs.BindRuntimeReferences(
            sr,
            cRt,
            vpRt,
            pool,
            grid.constraintCount,
            grid.cellSize,
            grid.spacing,
            grid.padding);

        return vs;
    }

    private static void StretchToParent(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
    }
}
