using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public BasePanel pausePanel;
    public Stack<BasePanel> panelsStack = new Stack<BasePanel>();
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pausePanel.Close();
    }
    private void OnEnable()
    {
        InputManger.PauseEvent += TogglePausePanel;
    }
    private void OnDisable()
    {
        InputManger.PauseEvent -= TogglePausePanel;
    }
    public void PushIn(BasePanel newPanel)
    {
        Debug.Log("入栈了");
        if (newPanel==null)
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
    public void TogglePausePanel()
    {
        PushIn(pausePanel);
    }
}