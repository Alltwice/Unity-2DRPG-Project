using UnityEngine;

public class MainMenuUI_Test : MonoBehaviour
{
    public void OnClickToGame()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.panelsStack.Clear();

        Time.timeScale = 1;
        LoadManager.Instance.StartLoading("MainScene", true);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickSavePanel()
    {
        if (UIManager.Instance == null)
            return;

        UIManager.Instance.ToggleSavePanel(PanelType.savePanel);
    }
}
