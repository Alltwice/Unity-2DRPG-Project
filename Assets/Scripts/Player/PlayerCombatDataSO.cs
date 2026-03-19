using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerCombatData", menuName = "数据/角色数据/战斗数据", order = 1)]
public class PlayerCombatDataSO : ScriptableObject
{
    [Header("战斗数据")]
    [SerializeField] private int damage;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private LayerMask attackLayer;
    [Header("战斗反馈")]
    [SerializeField] private float hitStopStunTime;
    [SerializeField] private float cameraShakeForce;
    
    public int Damage => damage;
    public float HitStopStunTime => hitStopStunTime;
    public float CameraShakeForce => cameraShakeForce;
    public Vector2 AttackRange => attackRange;
    public LayerMask AttackLayer => attackLayer;
}
