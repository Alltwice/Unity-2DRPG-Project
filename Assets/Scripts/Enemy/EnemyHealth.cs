using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void ChangeHealth(int changeHealth)
    {
        Debug.Log("造成伤害了");
        currentHealth -= changeHealth;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
