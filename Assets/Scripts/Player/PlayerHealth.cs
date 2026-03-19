using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth;
    public int maxHealth;
    public Vector2 attackObject;
    public float hitStopStunTime;
    public float cameraShakeForce;
    public PlayerDefence playerDefence;
    public int finalDamage;
    public PlayerDodge playerDodge;
    public void Awake()
    {
        playerDefence = GetComponent<PlayerDefence>();
        playerDodge = GetComponent<PlayerDodge>();
    }
    public void PlayerHealthInitialize(int maxHealth, float hitStopStunTime, float cameraShakeForce)
    {
        this.maxHealth = maxHealth;
        this.hitStopStunTime = hitStopStunTime;
        this.cameraShakeForce = cameraShakeForce;
        currentHealth = maxHealth;
    }
    public void Start()
    {
        GameEvent.TriggerPlayerHealthChange(currentHealth, maxHealth);
    }
    //一个改变生命值的方法
    public void ChangeHealth(int changeamount,Vector2 attackObject)
    {
        if(playerDodge.isRoll==true)
        {
            return;
        }
        this.attackObject = attackObject;
        finalDamage = playerDefence.FinallyDamage(changeamount);
        currentHealth -= finalDamage;
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
        HitStopManager.Instance.HitStop(hitStopStunTime);
    }
}
