using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDie = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UIManger.Instance.EnemyHealthChange(maxHealth, currentHealth);
    }
    public void ChangeHealth(int changeHealth)
    {
        currentHealth -= changeHealth;
        UIManger.Instance.EnemyHealthChange(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            isDie = true;
        }
        else if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
