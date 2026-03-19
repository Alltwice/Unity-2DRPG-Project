    using UnityEngine;
    [CreateAssetMenu(fileName = "NewPlayerBaseData", menuName = "数据/角色数据/基础数据", order = 0)]
    public class PlayerBaseDataSO : ScriptableObject
    {
        [Header("基础属性")]
        [SerializeField] private int moveSpeed = 5;
        [SerializeField] private int maxHealth = 30;
        public int MaxHealth => maxHealth;
        public int MoveSpeed => moveSpeed;
    }
