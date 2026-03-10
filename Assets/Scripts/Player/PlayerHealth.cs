using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public void Start()
    {
        currentHealth = maxHealth;
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
    //一个改变生命值的方法
    public void changeHealth(int changeamount)
    {
        currentHealth -= changeamount;
        if (currentHealth<=0)
        {
            gameObject.SetActive(false);
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
}
