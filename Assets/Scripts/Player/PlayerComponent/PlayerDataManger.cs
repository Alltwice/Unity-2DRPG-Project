using UnityEngine;

public class PlayerDataManger : MonoBehaviour
{
    [SerializeField] private PlayerBaseDataSO playerBaseDataSO;
    [SerializeField] private PlayerCombatDataSO playerCombatDataSO;
    private PlayerHealth playerHealth;
    private PlayerCombat playerCombat;
    private PlayerCombatFeedBack playerCombatFeedBack;
    private PlayerDefence playerDefence;
    private PlayerDodge playerDodge;
    public float MoveSpeed => playerBaseDataSO.MoveSpeed;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCombat = GetComponent<PlayerCombat>();
        playerCombatFeedBack = GetComponent<PlayerCombatFeedBack>();
        playerDefence = GetComponent<PlayerDefence>();
        playerDodge = GetComponent<PlayerDodge>();
        playerHealth.PlayerHealthInitialize(playerBaseDataSO.MaxHealth, playerCombatDataSO.HitStopStunTime, playerCombatDataSO.CameraShakeForce);
        playerCombat.PlayerCombatInitialize(playerCombatDataSO.Damage, playerCombatDataSO.AttackRange, playerCombatDataSO.AttackLayer);
        playerCombatFeedBack.PlayerCombatFeedBackInitialize(playerCombatDataSO.FlashDuration, playerCombatDataSO.ShakeDuration, playerCombatDataSO.ShakeStrength,playerCombatDataSO.FlashColor,
                                                            playerCombatDataSO.BlockFlashDuration,playerCombatDataSO.BlockShakeDuration,playerCombatDataSO.BlockShakeStrength,playerCombatDataSO.BlockFlashColor);
        playerDefence.PlayerDefenceInitialize(playerCombatDataSO.DamageReduction);
        playerDodge.PlayerDodgeInitialize(playerBaseDataSO.RollSpeed, playerBaseDataSO.RollColdDown);
    }
}
