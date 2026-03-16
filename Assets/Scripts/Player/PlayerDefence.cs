using DG.Tweening.Core.Easing;
using UnityEngine;

public class PlayerDefence : MonoBehaviour
{
    public Collider2D shieldCollider;
    public PlayerWarrior player;
    public PlayerHealth playerHealth;
    public float damageReduction = 0.3f;
    public bool isBlocking = false;
    private void Awake()
    {
        player = GetComponent<PlayerWarrior>();
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
        player.ChangeState(player.defenceStage);
    }
    public void StopDefence()
    {
        shieldCollider.enabled = false;
        isBlocking = false;
        player.ChangeStateIdle();
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
