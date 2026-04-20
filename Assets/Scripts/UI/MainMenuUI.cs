using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnClickToGame()
    {
        GameEvent.TriggerClickUISfx();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.panelsStack.Clear();
        }
            Time.timeScale = 1;
        LoadManager.Instance.StartLoading("MainScene", true);
    }
    public void OnClickExit()
    {
        GameEvent.TriggerClickUISfx();
        // 预编译指令：根据环境执行不同的退出方式
#if UNITY_EDITOR
        // 如果是在编辑器里运行，则停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 如果是打包出来的程序，则正常关闭
            Application.Quit();
#endif
    }

    public void OnClickSavePanel()
    {
        GameEvent.TriggerClickUISfx();
        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.ToggleSavePanel(PanelType.savePanel);
    }

    public void OnClickOpenSettings()
    {
        GameEvent.TriggerClickUISfx();
        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.ToggleSettingPanel(PanelType.settingPanel);
    }
}
