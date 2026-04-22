using UnityEngine;
/// <summary>
/// 纯粹记录增益属性
/// </summary>
public class TestPlayerStatModifiers : MonoBehaviour
{
    public float DamageReductionPercent { get; private set; }
    public int DamageBonus { get; private set; }
    public float MoveSpeedBonus { get; private set; }

    public void AddModifier(float damageReductionPercent, int damageBonus, float moveSpeedBonus)
    {
        DamageReductionPercent += damageReductionPercent;
        DamageBonus += damageBonus;
        MoveSpeedBonus += moveSpeedBonus;
        // 防止配置叠加后出现 100% 及以上免伤，给受击流程保留最低伤害空间。
        DamageReductionPercent = Mathf.Clamp01(DamageReductionPercent);
    }

    public void RemoveModifier(float damageReductionPercent, int damageBonus, float moveSpeedBonus)
    {
        DamageReductionPercent -= damageReductionPercent;
        DamageBonus -= damageBonus;
        MoveSpeedBonus -= moveSpeedBonus;
        DamageReductionPercent = Mathf.Clamp01(DamageReductionPercent);
    }

    public void ClearAll()
    {
        DamageReductionPercent = 0f;
        DamageBonus = 0;
        MoveSpeedBonus = 0f;
    }
}
