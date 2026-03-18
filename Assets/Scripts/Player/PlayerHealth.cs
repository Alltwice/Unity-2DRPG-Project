using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth=8;
    public int maxHealth=8;
    public Vector2 attackObject;
    public float stunTime;
    public float cameraShakeForce;
    public PlayerDefence playerDefence;
    public int finallDamage;
    public PlayerDoge playerDoge;
    public void Awake()
    {
        currentHealth = maxHealth;
        playerDefence = GetComponent<PlayerDefence>();
        playerDoge = GetComponent<PlayerDoge>();
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
    //一个改变生命值的方法
    public void ChangeHealth(int changeamount,Vector2 attackObject)
    {
        if(playerDoge.isRoll==true)
        {
            return;
        }
        this.attackObject = attackObject;
        finallDamage = playerDefence.FinallyDamage(changeamount);
        currentHealth -= finallDamage;
        //相机抖动
        GameEvent.TriggerCameraShake(cameraShakeForce);
        if (currentHealth<=0)
        {
            gameObject.SetActive(false);
            GameEvent.TriggerPlayerDeath();
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        //给UI组件提供数值变化，以及会调用一次受击反馈
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
        //调用playerController里的订阅事件，切换到受击状态
        GameEvent.TriggerPlayerHited();
        //顿帧效果
        HitStopManager.Instance.HitStop(stunTime);
    }
}
