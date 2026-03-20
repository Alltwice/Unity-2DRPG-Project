using DG.Tweening.Core.Easing;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerDefence : MonoBehaviour
{
    public Collider2D shieldCollider;
    public PlayerController player;
    public PlayerHealth playerHealth;
    public float damageReduction = 0.3f;
    public bool isBlocking = false;
    public void PlayerDefenceInitialize(float damageReduction)
    {
        this.damageReduction = damageReduction;
    }
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        InputManger.PushDefenceEvent +=StartDefence;
        InputManger.CanceldDefenceEvent +=StopDefence;
    }
    private void OnDisable()
    {
        InputManger.PushDefenceEvent -=StartDefence;
        InputManger.CanceldDefenceEvent -=StopDefence;
    }
    public void StartDefence()
    {
        shieldCollider.enabled = true;
        isBlocking = true;
    }
    public void StopDefence()
    {
        shieldCollider.enabled = false;
        isBlocking = false;
    }
    public int FinallyDamage(int damage)
    {
        if(isBlocking==true)
        {
            GameEvent.TriggerPlaySFX(GameEvent.SFXType.PlayerDefenceBeHit);
            return Mathf.Max(1, Mathf.RoundToInt(damage * damageReduction));
        }
        else
        {
            GameEvent.TriggerPlaySFX(GameEvent.SFXType.PlayerBeHit);
            return damage;
        }
    }
}
