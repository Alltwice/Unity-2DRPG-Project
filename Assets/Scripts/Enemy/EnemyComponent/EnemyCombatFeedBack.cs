using UnityEngine;
using DG.Tweening;
using System.Collections;

public class EnemyCombatFeedback : MonoBehaviour
{
    [SerializeField] private EnemyCombatDataSO CombatData;
    private SpriteRenderer sr;
    private int _lastHealth; // 记录上次血量，用于判断是否是“扣血”
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        enemyHealth=GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        enemyHealth.EnemyHealthChange += OnHealthChanged;
    }

    private void OnDisable()
    {
        enemyHealth.EnemyHealthChange -= OnHealthChanged;
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
        sr.color = CombatData.FlashColor;
        sr.DOColor(Color.white, CombatData.FlashDuration).SetEase(Ease.OutQuad);
        transform.DOShakePosition(CombatData.ShakeStrength, CombatData.ShakeDuration);
    }
}
