using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyCombatData", menuName = "数据/敌人数据/战斗数据", order = 1)]
public class EnemyCombatDataSO : ScriptableObject
{
    [Header("战斗反馈")]
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 0.15f;
    [SerializeField] private float hitStopTime=0.13f;
    [SerializeField] private float cameraShakeForce = 0.2f;
    public float FlashDuration => flashDuration;
    public Color FlashColor => flashColor;
    public float ShakeDuration => shakeDuration;
    public float ShakeStrength => shakeStrength;
    public float HitStopTime => hitStopTime;
    public float CameraShakeForce => cameraShakeForce;
}
