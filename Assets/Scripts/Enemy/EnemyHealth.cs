using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDie = false;

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
            Debug.Log("死了");
            isDie = true;
        }
        else if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
