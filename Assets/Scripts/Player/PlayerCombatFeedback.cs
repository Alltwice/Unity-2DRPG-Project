using UnityEngine;
using DG.Tweening;
using System.Collections;
/// <summary>
/// 当被击中后闪烁和晃动
/// </summary>
public class PlayerCombatFeedback : MonoBehaviour
{
    public float flashDuration = 0.2f;
    public Color flashColor = Color.red; 
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.15f;
    private SpriteRenderer sr;
    private int _lastHealth; // 记录上次血量，用于判断是否是“扣血”
    public PlayerDefence playerDefence;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        playerDefence = GetComponent<PlayerDefence>();
    }

    private void OnEnable()
    {
        GameEvent.PlayerHealthChange += OnHealthChanged;
    }

    private void OnDisable()
    {
        GameEvent.PlayerHealthChange -= OnHealthChanged;
    }

    private void Start()
    {
    }
    /// <summary>
    /// 当玩家血量变换时会调用这个判断逻辑，如果扣血就会掉用受击反馈
    /// </summary>
    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        // 如果当前血量比之前少，说明挨打了！触发反馈！
        if (currentHealth < _lastHealth)
        {
            if(playerDefence.isBlocking==true)
            {
                TriggerBlockHitFeedBack();
            }
            else
            {
                TriggerHitFeedBack();
            }
        }
        _lastHealth = currentHealth; // 更新记录
    }
    /// <summary>
    /// 触发受击效果的具体逻辑
    /// </summary>
    public void TriggerHitFeedBack()
    {
        sr.DOKill();
        sr.color = Color.red;
        sr.DOColor(Color.white, flashDuration).SetEase(Ease.OutQuad);
        transform.DOShakePosition(shakeStrength, shakeDuration);
    }
    public void TriggerBlockHitFeedBack()
    {
        sr.DOKill();
        sr.color = Color.orange;
        sr.DOColor(Color.white, 0.15f).SetEase(Ease.OutQuad);
        transform.DOShakePosition(0.10f, 0.15f);
    }
}
