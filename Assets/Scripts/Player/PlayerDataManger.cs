using UnityEngine;

public class PlayerDataManger : MonoBehaviour
{
    [SerializeField] PlayerBaseDataSO playerBaseDataSO;
    [SerializeField] PlayerCombatDataSO playerCombatDataSO;
    private PlayerHealth playerHealth;
    private PlayerCombat playerCombat;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCombat = GetComponent<PlayerCombat>();
        playerHealth.PlayerHealthInitialize(playerBaseDataSO.MaxHealth, playerCombatDataSO.HitStopStunTime, playerCombatDataSO.CameraShakeForce);
        playerCombat.PlayerCombatInitialize(playerCombatDataSO.Damage, playerCombatDataSO.AttackRange, playerCombatDataSO.AttackLayer);
    }
}
