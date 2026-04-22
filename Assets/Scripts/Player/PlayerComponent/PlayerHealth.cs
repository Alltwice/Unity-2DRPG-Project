using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerBaseDataSO baseData;
    [SerializeField] private PlayerCombatDataSO combatData;

    public int currentHealth = 8;
    public Vector2 attackObject;
    public PlayerDefence playerDefence;
    public PlayerDodge playerDodge;
    private TestPlayerStatModifiers statModifiers;
    public int finallDamage;
    // 当前版本取消“装备提高生命上限”，生命上限固定为基础值。
    public int EffectiveMaxHealth => Mathf.Max(1, baseData.MaxHealth);
    public float EffectiveDamageReductionPercent => statModifiers != null ? Mathf.Clamp01(statModifiers.DamageReductionPercent) : 0f;
    public void Awake()
    {
        statModifiers = GetComponent<TestPlayerStatModifiers>();
        if (statModifiers == null)
            statModifiers = gameObject.AddComponent<TestPlayerStatModifiers>();

        currentHealth = EffectiveMaxHealth;
        playerDefence = GetComponent<PlayerDefence>();
        playerDodge = GetComponent<PlayerDodge>();
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, EffectiveMaxHealth);
    }
    //һ���ı�����ֵ�ķ���
    public void ReduceHealth(int changeamount, Vector2 attackObject)
    {
        if(playerDodge.isRoll==true)
        {
            return;
        }
        this.attackObject = attackObject;
        finallDamage = playerDefence.FinallyDamage(changeamount);
        // 在格挡减伤之后，再叠加装备提供的百分比减伤。
        if (finallDamage > 0 && EffectiveDamageReductionPercent > 0f)
        {
            finallDamage = Mathf.Max(1, Mathf.RoundToInt(finallDamage * (1f - EffectiveDamageReductionPercent)));
        }
        currentHealth -= finallDamage;
        GameEvent.TriggerCameraShake(combatData.CameraShakeForce);
        HitStopManager.Instance.HitStop(combatData.HitStopStunTime);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameEvent.TriggerPlayerDeath();
        }
        else if (currentHealth > EffectiveMaxHealth)
        {
            currentHealth = EffectiveMaxHealth;
        }

        //UI���ݸ��º��ܻ���������
        GameEvent.TriggerPlayerHealthChange(currentHealth, EffectiveMaxHealth);
        //�����л���ø÷����л����ܻ�״̬
        GameEvent.TriggerPlayerHited();
    }
    public void HealHealth(int changeamount)
    {
        currentHealth += changeamount;
        if (currentHealth > EffectiveMaxHealth) 
        {
            currentHealth = EffectiveMaxHealth;
        }
        GameEvent.TriggerPlayerHealthChange(currentHealth, EffectiveMaxHealth);
    }

    /// <summary>
    /// 外部（例如读档）直接同步血量，只更新数值和UI，不触发受击链路。
    /// </summary>
    public void SyncHealthOnly(int targetHealth)
    {
        currentHealth = Mathf.Clamp(targetHealth, 0, EffectiveMaxHealth);
        GameEvent.TriggerPlayerHealthSyncOnly(currentHealth, EffectiveMaxHealth);
    }

    public void RecalculateAndClamp()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, EffectiveMaxHealth);
        GameEvent.TriggerPlayerHealthChange(currentHealth, EffectiveMaxHealth);
    }
}
