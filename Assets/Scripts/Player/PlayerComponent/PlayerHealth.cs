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
    public int finallDamage;
    public void Awake()
    {
        currentHealth = baseData.MaxHealth;
        playerDefence = GetComponent<PlayerDefence>();
        playerDodge = GetComponent<PlayerDodge>();
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, baseData.MaxHealth);
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
        currentHealth -= finallDamage;
        GameEvent.TriggerCameraShake(combatData.CameraShakeForce);
        HitStopManager.Instance.HitStop(combatData.HitStopStunTime);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameEvent.TriggerPlayerDeath();
        }
        else if (currentHealth > baseData.MaxHealth)
        {
            currentHealth = baseData.MaxHealth;
        }

        //UI���ݸ��º��ܻ���������
        GameEvent.TriggerPlayerHealthChange(currentHealth, baseData.MaxHealth);
        //�����л���ø÷����л����ܻ�״̬
        GameEvent.TriggerPlayerHited();
    }
    public void HealHealth(int changeamount)
    {
        currentHealth += changeamount;
        if (currentHealth > baseData.MaxHealth) 
        {
            currentHealth = baseData.MaxHealth;
        }
        GameEvent.TriggerPlayerHealthChange(currentHealth, baseData.MaxHealth);
    }

    /// <summary>
    /// 外部（例如读档）直接同步血量，只更新数值和UI，不触发受击链路。
    /// </summary>
    public void SyncHealthOnly(int targetHealth)
    {
        currentHealth = Mathf.Clamp(targetHealth, 0, baseData.MaxHealth);
        GameEvent.TriggerPlayerHealthSyncOnly(currentHealth, baseData.MaxHealth);
    }
}
