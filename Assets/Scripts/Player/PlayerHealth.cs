using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public TMP_Text healthText;
    public Animator animatorHealth;
    //在开始时显示文本内容
    public void Start()
    {
        healthText.text = currentHealth + "/" + maxHealth;
    }
    //一个改变生命值的方法
    public void changeHealth(int changeamount)
    {
        currentHealth += changeamount;
        animatorHealth.Play("HealthText");
        healthText.text = currentHealth + "/" + maxHealth;
        if (currentHealth<=0)
        {
            gameObject.SetActive(false);
        }
    }
}
