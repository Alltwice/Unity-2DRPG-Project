using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyBaseData", menuName = "数据/敌人数据/基础数据", order = 0)]
public class EnemyBaseDataSO : ScriptableObject
{
    [Header("基础数据")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int maxHealth;
    public float MoveSpeed => moveSpeed;
    public int Damage => damage;
    public int MaxHealth => maxHealth;
}
