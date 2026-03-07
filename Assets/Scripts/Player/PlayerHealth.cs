using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public TMP_Text healthText;
    public UIManger ui;
    //在开始时显示文本内容
    public void Awake()
    {
        ui = GameObject.Find("UIManger").GetComponent<UIManger>();
    }
    public void Start()
    {
        ui.PlayerHealthChange(maxHealth,currentHealth);
    }
    //一个改变生命值的方法
    public void changeHealth(int changeamount)
    {
        currentHealth -= changeamount;
        ui.PlayerHealthChange(maxHealth,currentHealth);
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
