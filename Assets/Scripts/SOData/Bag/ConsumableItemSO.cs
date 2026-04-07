using UnityEngine;
[CreateAssetMenu(fileName = "NewConsumable", menuName = "数据/物品数据/消耗品", order = 3)]
public class ConsumableItemSO : ItemDataSO
{
    [Header("消耗品属性")]
    [SerializeField] private int healAmount=10;
    public override void UseMethod(GameObject targer)
    {
        PlayerHealth playerHealth=targer.GetComponent<PlayerHealth>();
        playerHealth.HealHealth(healAmount);
    }
}
