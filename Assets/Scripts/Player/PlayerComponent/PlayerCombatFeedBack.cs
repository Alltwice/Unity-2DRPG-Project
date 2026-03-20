using UnityEngine;
using DG.Tweening;
using System.Collections;
/// <summary>
/// 当被击中后闪烁和晃动
/// </summary>
public class PlayerCombatFeedBack : MonoBehaviour
{
    private float flashDuration;
    private float blockFlashDuration;
    private Color flashColor;
    private Color blockFlashColor;
    private float shakeDuration;
    private float blockShakeDuration;
    private float shakeStrength;
    private float blockShakeStrength;
    private SpriteRenderer sr;
    private int _lastHealth; // 记录上次血量，用于判断是否是“扣血”
    private PlayerDefence playerDefence;
    public void PlayerCombatFeedBackInitialize(float flashDuration, float shakeDuration, float shakeStrength, Color flashColor,
                                               float blockFlashDuration,float blockShakeDuration,float blockShakeStrength,Color blockFlashColor)
    {
        this.flashDuration = flashDuration;
        this.shakeDuration = shakeDuration;
        this.shakeStrength = shakeStrength;
        this.flashColor = flashColor;
        this.blockFlashDuration = blockFlashDuration;
        this.blockFlashColor = blockFlashColor;
        this.blockShakeDuration = blockShakeDuration;
        this.blockShakeStrength = blockShakeStrength;
    }
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
        sr.color = flashColor;
        sr.DOColor(Color.white, flashDuration).SetEase(Ease.OutQuad);
        transform.DOShakePosition(shakeStrength, shakeDuration);
    }
    public void TriggerBlockHitFeedBack()
    {
        sr.DOKill();
        sr.color = blockFlashColor;
        sr.DOColor(Color.white, blockFlashDuration).SetEase(Ease.OutQuad);
        transform.DOShakePosition(blockShakeStrength, blockShakeDuration);
    }
}
