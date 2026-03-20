using DG.Tweening.Core.Easing;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerDefence : MonoBehaviour
{
    [SerializeField] private PlayerCombatDataSO combatData;

    public Collider2D shieldCollider;
    public PlayerController player;
    public PlayerHealth playerHealth;
    public bool isBlocking = false;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        InputManger.PushDefenceEvent += StartDefence;
        InputManger.CanceldDefenceEvent += StopDefence;
    }
    private void OnDisable()
    {
        InputManger.PushDefenceEvent -= StartDefence;
        InputManger.CanceldDefenceEvent -= StopDefence;
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
        if (isBlocking == true)
        {
            GameEvent.TriggerPlaySFX(GameEvent.SFXType.PlayerDefenceBeHit);
            return Mathf.Max(1, Mathf.RoundToInt(damage * combatData.DamageReduction));
        }
        else
        {
            GameEvent.TriggerPlaySFX(GameEvent.SFXType.PlayerBeHit);
            return damage;
        }
    }
}
