using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class PauseUI : BasePanel
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
        bgm.volume = volumeSlider.value;
        bgmPercent.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }
    public void ClickClose()
    {
        UIManager.Instance.PopOut();
    }
    public void OnClickRestart()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.panelsStack.Clear();
        }
        LoadManager.Instance.StartLoading("MainScene", true);
    }
    public void OnClickExit()
    {
        // 预编译指令：根据环境执行不同的退出方式
#if UNITY_EDITOR
        // 如果是在编辑器里运行，则停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 如果是打包出来的程序，则正常关闭
            Application.Quit();
#endif
    }
    public void OnClickToMenu()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.panelsStack.Clear();
        }
        Time.timeScale = 1;
        LoadManager.Instance.StartLoading("Menu", true);
    }
}
