using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
public enum PanelType
{
    pausePanel,
    gameOverPanel,
    settingPanel,
    bagPanel,
    savePanel
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    public Stack<BasePanel> panelsStack = new Stack<BasePanel>();
    public bool isPause;
    private bool isDeath;
    private bool bagOpen;
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        isPause = false;
        isDeath = false;
        SettingsPanelUI.ApplyStoredVideoMode();
    }
    //使用字典注册减少对其他组件的依赖
    public void RegisterPanel(PanelType type, BasePanel panel)
    {
        if (!panelDict.ContainsKey(type))
        {
            panelDict.Add(type, panel);
        }
    }

    public void UnregisterPanel(PanelType type)
    {
        if (panelDict.ContainsKey(type))
        {
            panelDict.Remove(type);
        }
    }
    private void OnEnable()
    {
        InputManger.PauseEvent += TogglePausePanel;
        GameEvent.PlayerDeath += PlayerDeath;
        InputManger.OpenBagEvent += ToggleBagPanel;
    }
    private void OnDisable()
    {
        InputManger.PauseEvent -= TogglePausePanel;
        GameEvent.PlayerDeath -= PlayerDeath;
        InputManger.OpenBagEvent -= ToggleBagPanel;
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
        if (topPanel.panelType == PanelType.pausePanel)
        {
            // 关键修复：不论是按 Esc 还是点暂停面板按钮，弹出暂停面板后都必须复位 isPause
            isPause = false;
        }
        topPanel.Close();
        panelsStack.Pop();
        if(panelsStack.Count>0)
        {
            BasePanel nextPanel = panelsStack.Peek();
            nextPanel.Resume();
        }
    }
    public void TogglePausePanel(PanelType pausePanel)
    {
        if (panelDict.TryGetValue(pausePanel,out BasePanel targetPanel)==false)
        {
            return;
        }
        if(panelsStack.Count>0)
        {
            if(isDeath==true)
            {
                return;
            }
            if (panelsStack.Peek() == targetPanel)
            {
                isPause = false;
            }
            PopOut();
        }
        else
        {
            isPause = true;
            PushIn(targetPanel);
        }
    }
    public void PlayerDeath()
    {
        ToggleGameOverPanel(PanelType.gameOverPanel);
    }
    public void ToggleGameOverPanel(PanelType gameOverPanel)
    {
        if (panelDict.TryGetValue(gameOverPanel, out BasePanel targetPanel) == false)
        {
            return;
        }
        isDeath = true;
        PushIn(targetPanel);
    }
    public void ToggleBagPanel(PanelType bagPanel)
    {
        if(isPause==true||isDeath==true)
        {
            return;
        }
        if(panelDict.TryGetValue(bagPanel,out BasePanel targetPanel)==false)
        {
            return;
        }
        if(panelsStack.Count>0&&panelsStack.Peek()==targetPanel)
        {
            PopOut();
            GameEvent.TriggerBagClose();
        }
        else
        {
            PushIn(targetPanel);
        }
    }

    public void ToggleSavePanel(PanelType savePanel)
    {
        if (isDeath == true)
        {
            return;
        }

        if (panelDict.TryGetValue(savePanel, out BasePanel targetPanel) == false)
        {
            return;
        }

        if (panelsStack.Count > 0 && panelsStack.Peek() == targetPanel)
        {
            PopOut();
        }
        else
        {
            PushIn(targetPanel);
        }
    }

    public void ToggleSettingPanel(PanelType settingPanelType)
    {
        if (isDeath == true)
        {
            return;
        }

        if (panelDict.TryGetValue(settingPanelType, out BasePanel targetPanel) == false)
        {
            return;
        }

        if (panelsStack.Count > 0 && panelsStack.Peek() == targetPanel)
        {
            PopOut();
        }
        else
        {
            PushIn(targetPanel);
        }
    }
}