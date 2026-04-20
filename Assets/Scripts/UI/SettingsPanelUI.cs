using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : BasePanel
{
    [Header("音频")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI bgmPercentText;
    [SerializeField] private TextMeshProUGUI sfxPercentText;
    [SerializeField] private AudioManager audioManager;

    [Header("显示")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    //专门装着分辨率显示的容器 
    private readonly List<Resolution> _uniqueResolutions = new List<Resolution>();

    // 静态方法：游戏启动时调用，无需打开面板即可生效，负责设置游戏窗口大小
    public static void ApplyStoredVideoMode()
    {
        if (PlayerPrefs.HasKey("FullscreenPrefs"))
        {
            bool isFull = PlayerPrefs.GetInt("FullscreenPrefs") == 1;
            Screen.fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        if (PlayerPrefs.HasKey("ResWidthPrefs") && PlayerPrefs.HasKey("ResHeightPrefs"))
        {
            int w = PlayerPrefs.GetInt("ResWidthPrefs");
            int h = PlayerPrefs.GetInt("ResHeightPrefs");
            Screen.SetResolution(w, h, Screen.fullScreenMode);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        BuildUniqueResolutions();

        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        // 绑定事件监听
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        if (fullscreenToggle != null) fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        if (resolutionDropdown != null) resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
    }
    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.settingPanel, this);
        RefreshAllControlsFromState();
    }
    private void OnDisable()
    {
        UIManager.Instance.UnregisterPanel(PanelType.settingPanel);
    }
    public override void Open()
    {
        base.Open();
    }

    private void RefreshAllControlsFromState()
    {
        // 直接读取并赋值
        // 注意：这里的直接赋值会触发一次下方的 OnValueChanged 事件
        //打开面板还会保存一次

        if (bgmSlider != null) bgmSlider.value = PlayerPrefs.GetFloat("BgmVolumePrefs", 1f);
        if (sfxSlider != null) sfxSlider.value = PlayerPrefs.GetFloat("SfxVolumePrefs", 1f);

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreenMode != FullScreenMode.Windowed;
        }

        if (resolutionDropdown != null && _uniqueResolutions.Count > 0)
        {
            int idx = FindResolutionIndex(Screen.width, Screen.height);
            resolutionDropdown.value = Mathf.Max(0, idx); // 找不到就默认显示第一个
        }

        UpdatePercentLabels();
    }

    //Bgm滑条变化
    private void OnBgmSliderChanged(float value)
    {
        UpdatePercentLabels();
        PlayerPrefs.SetFloat("BgmVolumePrefs", value);
        PlayerPrefs.Save();
        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager != null)
            audioManager.SetBgmVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        UpdatePercentLabels();
        PlayerPrefs.SetFloat("SfxVolumePrefs", value);
        PlayerPrefs.Save();
        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager != null)
            audioManager.SetSfxVolume(value);
    }

    private void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetInt("FullscreenPrefs", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnResolutionDropdownChanged(int index)
    {
        if (index < 0 || index >= _uniqueResolutions.Count) return;

        Resolution r = _uniqueResolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);

        PlayerPrefs.SetInt("ResWidthPrefs", r.width);
        PlayerPrefs.SetInt("ResHeightPrefs", r.height);
        PlayerPrefs.Save();
    }

    // -----------------------------------------
    // 辅助工具方法
    // -----------------------------------------
    //设置百分比
    private void UpdatePercentLabels()
    {
        if (bgmPercentText != null) bgmPercentText.text = $"{"背景音量"+Mathf.RoundToInt(bgmSlider.value * 100f)}%";
        if (sfxPercentText != null) sfxPercentText.text = $"{"音效音量"+Mathf.RoundToInt(sfxSlider.value * 100f)}%";
    }
    //自动寻找电脑适合的分辨率
    private void BuildUniqueResolutions()
    {
        _uniqueResolutions.Clear();
        var best = new Dictionary<(int w, int h), Resolution>();
        //找到屏幕支持的所有分辨率
        foreach (Resolution r in Screen.resolutions)
        {
            var key = (r.width, r.height);
            // 过滤重复尺寸，只保留最高刷新率
            if (!best.TryGetValue(key, out Resolution cur) || r.refreshRateRatio.value > cur.refreshRateRatio.value)
                best[key] = r;
        }
        //留下最高刷新率的放入容器
        foreach (Resolution r in best.Values) _uniqueResolutions.Add(r);

        // 按屏幕面积从小到大排序
        _uniqueResolutions.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));

        if (resolutionDropdown == null) return;
        //清空之前的选项
        resolutionDropdown.ClearOptions();
        var labels = new List<string>();
        //放入新选项并拼接字符串显示
        foreach (Resolution r in _uniqueResolutions)
            labels.Add($"{r.width} x {r.height}");
        //加入下拉菜单
        resolutionDropdown.AddOptions(labels);
    }
    //给每一个下拉菜单标号
    private int FindResolutionIndex(int width, int height)
    {
        for (int i = 0; i < _uniqueResolutions.Count; i++)
        {
            if (_uniqueResolutions[i].width == width && _uniqueResolutions[i].height == height)
                return i;
        }
        return -1;
    }

    public void OnClickClose()
    {
        // 触发关闭 UI 的音效
        if (UIManager.Instance != null) UIManager.Instance.PopOut();
    }
}