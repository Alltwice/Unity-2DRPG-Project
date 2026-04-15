using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class PauseUI_Test : BasePanel
{
    public Button backGame;
    public TextMeshProUGUI bgmPercent;
    public Slider volumeSlider;
    public AudioSource bgm;

    protected override void Awake()
    {
        base.Awake();
        volumeSlider.value = bgm.volume;
        bgmPercent.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }

    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.pausePanel, this);
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UnregisterPanel(PanelType.pausePanel);
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Close()
    {
        Time.timeScale = 1;
        base.Close();
    }

    public void ChangeBgmVolum(float value)
    {
        bgm.volume = value;
        bgmPercent.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void ClickClose()
    {
        UIManager.Instance.PopOut();
    }

    public void OnClickRestart()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.panelsStack.Clear();

        LoadManager.Instance.StartLoading("MainScene", true);
    }

    public void OnClickToMenu()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.panelsStack.Clear();

        Time.timeScale = 1;
        LoadManager.Instance.StartLoading("Menu", true);
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
