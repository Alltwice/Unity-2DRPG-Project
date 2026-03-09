using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    //在开始时显示文本内容
    public void Awake()
    {
    }
    public void Start()
    {
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
    }
}
