using System;
using UnityEngine;
    [CreateAssetMenu(fileName = "NewPlayerBaseData", menuName = "数据/角色数据/基础数据", order = 0)]
    public class PlayerBaseDataSO : ScriptableObject
    {
        [Header("基础属性")]
        [SerializeField] private int moveSpeed;
        [SerializeField] private int maxHealth;
        [SerializeField] private float rollSpeed;
        [SerializeField] private float rollColdDown;
        public int MaxHealth => maxHealth;
        public int MoveSpeed => moveSpeed;
        public float RollSpeed => rollSpeed;
        public float RollColdDown => rollColdDown;
    }
