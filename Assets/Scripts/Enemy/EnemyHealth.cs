using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDie = false;
    public event Action<int, int> EnemyHealthChange;
    public event Action EnemyHited;
    public Vector2 attackObject;
    public float hitStopTime;
    public float cameraShakeForce=0.2f;
    private void Start()
    {
        currentHealth = maxHealth;
        TriggerEnemyHealthChange(currentHealth, maxHealth);
    }
    public void ChangeHealth(int changeHealth,Vector2 attackObject)
    {
        this.attackObject = attackObject;
        currentHealth -= changeHealth;
        GameEvent.TriggerCameraShake(cameraShakeForce);
        HitStopManager.Instance.HitStop(hitStopTime);
        GameEvent.TriggerPlaySFX(GameEvent.SFXType.EnemyHit);
        if (currentHealth <= 0)
        {
            isDie = true;
        }
        else if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
        TriggerEnemyHited();
        TriggerEnemyHealthChange(currentHealth, maxHealth);
    }
    public void TriggerEnemyHealthChange(int current, int max)
    {
        EnemyHealthChange?.Invoke(current, max);
    }
    public  void TriggerEnemyHited()
    {
        EnemyHited?.Invoke();
    }
}
