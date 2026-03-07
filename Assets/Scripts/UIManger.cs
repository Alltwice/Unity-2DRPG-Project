using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
    // 单例实例
    public static UIManger Instance { get; private set; }

    // 可在 Inspector 绑定（也保留了原有 field）
    public InputManger input;
    public GameObject pauseUI;
    public AudioSource BGM;
    public Slider BGMSilder;
    public Slider playerHealthSilder;
    public Slider enemyHealthSilder;
    public TMP_Text BGMText;
    public int Percent;

    private void Awake()
    {
        // 单例初始化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        input = GetComponent<InputManger>();
        BGM = GameObject.Find("Cameras")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (BGMSilder != null && BGM != null)
            BGMSilder.value = BGM.volume;
    }

    public void ChangeVolume(float value)
    {
        Percent = Mathf.RoundToInt(value * 100);
        if (BGMSilder != null && BGM != null)
            BGM.volume = BGMSilder.value;
        if (BGMText != null)
            BGMText.text = Percent + "%";
    }

    public void PlayerHealthChange(int maxHealth, int currentHealth)
    {
        if (playerHealthSilder != null)
        {
            playerHealthSilder.maxValue = maxHealth;
            playerHealthSilder.value = currentHealth;
        }
    }

    public void EnemyHealthChange(int maxHealth, int currentHealth)
    {
        if (enemyHealthSilder != null)
        {
            enemyHealthSilder.maxValue = maxHealth;
            enemyHealthSilder.value = currentHealth;
        }
    }

    // 公开的切换方法，供 InputManger 或其它脚本调用
    public void OpenPauseUI()
    {
        if (pauseUI == null)
        {
            Debug.LogWarning("[UIManger] pauseUI 未绑定。");
            return;
        }
        bool newState = !pauseUI.activeSelf;
        pauseUI.SetActive(newState);
        Debug.Log("[UIManger] Pause UI toggled -> " + newState);
    }

    // 静态便捷入口（可直接 UIManger.TogglePause()）
    public static void TogglePause()
    {
        if (Instance == null)
        {
            Debug.LogWarning("[UIManger] Instance is null when calling TogglePause()");
            return;
        }
        Instance.OpenPauseUI();
    }
}
