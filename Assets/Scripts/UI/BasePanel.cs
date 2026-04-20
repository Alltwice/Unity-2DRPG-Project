using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 面板基类：直接挂在 Hierarchy 里的 UI 对象上
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    public CanvasGroup canvas;
    public PanelType panelType;
    protected virtual void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        Close();
    }
    public virtual void Open()
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    public virtual void Close()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public virtual void Pause()
    {
        canvas.interactable = false;
    }
    public virtual void Resume()
    {
        canvas.interactable = true;
    }
}