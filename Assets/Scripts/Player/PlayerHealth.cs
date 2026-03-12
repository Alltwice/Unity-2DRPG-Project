using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth=8;
    public int maxHealth=8;
    public void Awake()
    {
        currentHealth = maxHealth;
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
    //一个改变生命值的方法
    public void changeHealth(int changeamount)
    {
        currentHealth -= changeamount;
        if (currentHealth<=0)
        {
            gameObject.SetActive(false);
            GameEvent.TriggerPlayerDeath();
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
}
