using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
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
        Cursor.visible = false;
        base.Close();
    }
    public override void Pause()
    {
        base.Pause();
    }
    public override void Resume()
    {
        base.Resume();
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
}
