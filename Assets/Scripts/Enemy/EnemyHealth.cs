using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDie = false;

    private void Start()
    {
        currentHealth = maxHealth;
        GameEvent.TriggerEnemyHealthChange(currentHealth, maxHealth);
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
        GameEvent.TriggerEnemyHealthChange(currentHealth, maxHealth);
    }
}
