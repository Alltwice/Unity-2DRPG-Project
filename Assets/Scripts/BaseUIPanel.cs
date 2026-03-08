using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 面板基类：直接挂在 Hierarchy 里的 UI 对象上
/// </summary>
public class BasePanel : MonoBehaviour
{
    public CanvasGroup canvas;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    public void Open()
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    public void Close()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public void Pause()
    {
        canvas.interactable = false;
    }
    public void Resume()
    {
        canvas.interactable = true;
    }
}
