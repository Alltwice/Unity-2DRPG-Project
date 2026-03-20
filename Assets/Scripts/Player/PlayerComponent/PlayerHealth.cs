using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerBaseDataSO baseData;
    [SerializeField] private PlayerCombatDataSO combatData;

    public int currentHealth = 8;
    public Vector2 attackObject;
    public PlayerDefence playerDefence;
    public int finallDamage;

    public void Awake()
    {
        currentHealth = baseData.MaxHealth;
        playerDefence = GetComponent<PlayerDefence>();
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, baseData.MaxHealth);
    }
    //一个改变生命值的方法
    public void ChangeHealth(int changeamount, Vector2 attackObject)
    {
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

        //UI数据更新和受击方法调动
        GameEvent.TriggerPlayerHealthChange(currentHealth, baseData.MaxHealth);
        //被打中会调用该方法切换到受击状态
        GameEvent.TriggerPlayerHited();
    }
}
