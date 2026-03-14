using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerCombatFeedback : MonoBehaviour
{
    public float flashDuration = 0.15f;
    public Color flashColor = Color.red; 
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.15f;
    private SpriteRenderer sr;
    private int _lastHealth; // 记录上次血量，用于判断是否是“扣血”

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        // 如果当前血量比之前少，说明挨打了！触发反馈！
        if (currentHealth < _lastHealth)
        {
            TriggerHitFeedback();
        }
        _lastHealth = currentHealth; // 更新记录
    }
    public void TriggerHitFeedback()
    {
        sr.DOKill();
        sr.color = Color.red;
        sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutQuad);
        transform.DOShakePosition(0.15f, 0.2f);
    }
}
