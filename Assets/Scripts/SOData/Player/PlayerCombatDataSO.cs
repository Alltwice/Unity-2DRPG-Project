using System;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerCombatData", menuName = "数据/角色数据/战斗数据", order = 1)]
public class PlayerCombatDataSO : ScriptableObject
{
    [Header("战斗数据")]
    [SerializeField] private int damage;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float damageReduction;
    [Header("战斗反馈")]
    [SerializeField] private float hitStopStunTime;
    [SerializeField] private float cameraShakeForce;
    [SerializeField] private float flashDuration;
    [SerializeField] private float blockFlashDuration;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float blockShakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float blockShakeStrength;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color blockflashColor;
    public float DamageReduction => damageReduction;
    public int Damage => damage;
    public float HitStopStunTime => hitStopStunTime;
    public float CameraShakeForce => cameraShakeForce;
    public Vector2 AttackRange => attackRange;
    public LayerMask AttackLayer => attackLayer;
    public float FlashDuration=> flashDuration;
    public float ShakeDuration => shakeDuration;
    public float ShakeStrength => shakeStrength;
    public Color FlashColor => flashColor;
    public float BlockFlashDuration => blockFlashDuration;
    public float BlockShakeDuration => blockShakeDuration;
    public float BlockShakeStrength => blockShakeStrength;
    public Color BlockFlashColor => blockflashColor;
}
