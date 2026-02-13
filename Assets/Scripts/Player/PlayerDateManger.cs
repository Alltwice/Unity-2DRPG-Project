using UnityEngine;

public class PlayerDateManger : MonoBehaviour
{
    public static PlayerDateManger instance;

    [Header("基础数据")]
    public float moveSpeed;
    public int damage;
    [HideInInspector] public bool isbacking = false;
    [Header("攻击范围")]
    public Transform attackPoint;
    public Vector2 attackRange;
    public LayerMask attackLayer;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
