using System;
using UnityEngine;

[RequireComponent(typeof(EnemySaveIdentity))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyBaseDataSO BaseData;
    [SerializeField] private EnemyCombatDataSO CombatData;
    public int currentHealth;
    public bool isDie = false;
    public event Action<int, int> EnemyHealthChange;
    public event Action EnemyHited;
    public Vector2 attackObject;
    private void Start()
    {
        currentHealth = BaseData.MaxHealth;
        TriggerEnemyHealthChange(currentHealth, BaseData.MaxHealth);
    }
    public void ChangeHealth(int changeHealth,Vector2 attackObject)
    {
        this.attackObject = attackObject;
        currentHealth -= changeHealth;
        GameEvent.TriggerPlaySFX(GameEvent.SFXType.EnemyBeHit);
        //相机晃动
        GameEvent.TriggerCameraShake(CombatData.CameraShakeForce);
        //顿帧
        HitStopManager.Instance.HitStop(CombatData.HitStopTime);
        if (currentHealth <= 0)
        {
            isDie = true;
        }
        else if(currentHealth>BaseData.MaxHealth)
        {
            currentHealth = BaseData.MaxHealth;
        }
        //切换受击状态
        TriggerEnemyHited();
        //给UI传值
        TriggerEnemyHealthChange(currentHealth, BaseData.MaxHealth);
    }
    public void TriggerEnemyHealthChange(int current, int max)
    {
        EnemyHealthChange?.Invoke(current, max);
    }
    public void TriggerEnemyHited()
    {
        EnemyHited?.Invoke();
    }

    public void SyncHealthOnly(int targetHealth)
    {
        int maxHealth = BaseData != null ? BaseData.MaxHealth : targetHealth;
        currentHealth = Mathf.Clamp(targetHealth, 0, maxHealth);
        isDie = currentHealth <= 0;
        TriggerEnemyHealthChange(currentHealth, maxHealth);
    }

    public void ApplySnapshot(int targetHealth, bool deadState)
    {
        SyncHealthOnly(targetHealth);
        isDie = deadState || currentHealth <= 0;
    }
}
