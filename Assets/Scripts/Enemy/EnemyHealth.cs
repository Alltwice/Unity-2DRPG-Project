using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDie = false;
    public event Action<int, int> EnemyHealthChange;

    private void Start()
    {
        currentHealth = maxHealth;
        TriggerEnemyHealthChange(currentHealth, maxHealth);
    }
    public void ChangeHealth(int changeHealth)
    {
        currentHealth -= changeHealth;
        if (currentHealth <= 0)
        {
            isDie = true;
        }
        else if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
        TriggerEnemyHealthChange(currentHealth, maxHealth);
    }
    public void TriggerEnemyHealthChange(int current, int max)
    {
        EnemyHealthChange?.Invoke(current, max);
    }
}
