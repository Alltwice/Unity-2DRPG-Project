using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
///  UI 管理器：管理场景中已有的 UI 面板
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public BasePanel PausePanel;
    public Stack<BasePanel> panelsStack = new Stack<BasePanel>();
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PausePanel.Close();
    }
    public void PushIn(BasePanel newPanel)
    {
        if(newPanel==null)
        {
            return;
        }
        if(panelsStack.Count>0)
        {
            BasePanel topPanel = panelsStack.Peek();
            topPanel.Pause();
        }
        newPanel.Open();
        panelsStack.Push(newPanel);
    }
    public void PopOut()
    {
        if(panelsStack.Count==0)
        {
            return;
        }
        BasePanel topPanel = panelsStack.Peek();
        topPanel.Close();
        panelsStack.Pop();
        if(panelsStack.Count>0)
        {
            BasePanel nextPanel = panelsStack.Peek();
            nextPanel.Resume();
        }
    }
}