using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public BasePanel pausePanel;
    public BasePanel gameOverPanel;
    public Stack<BasePanel> panelsStack = new Stack<BasePanel>();
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        pausePanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(true);
        pausePanel.Close();
        gameOverPanel.Close();
    }
    private void OnEnable()
    {
        InputManger.PauseEvent += TogglePausePanel;
        GameEvent.PlayerDeath += ToggleGameOverPanel;
    }
    private void OnDisable()
    {
        InputManger.PauseEvent -= TogglePausePanel;
        GameEvent.PlayerDeath -= ToggleGameOverPanel;
    }
    public void PushIn(BasePanel newPanel)
    {
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
        if(pausePanel.canvas.alpha==0)
        {
            PushIn(pausePanel);
        }
        else
        {
            PopOut();   
        }
    }
    public void ToggleGameOverPanel()
    {
        if(gameOverPanel.canvas.alpha==0)
        {
            PushIn(gameOverPanel);
        }
    }
}