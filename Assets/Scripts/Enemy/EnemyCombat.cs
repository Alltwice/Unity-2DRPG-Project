using TMPro;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayer;
    public float attackFroce;
    public float stunTime;

    private PlayerHealth pHp;
    private PlayerController pC;
    private int collisionDamage;
    //得到玩家的脚本

    private void Start()
    {
        pHp = GameObject.Find("Warrior_Player").GetComponent<PlayerHealth>();
        pC = GameObject.Find("Warrior_Player").GetComponent<PlayerController>();
    }
    //碰撞时引用脚本中的方法并传入伤害值
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collisionDamage = 1;
            pHp.changeHealth(-collisionDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    //攻击时调用脚本
    public void Attack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hit.Length > 0) 
        {
            collisionDamage = 1;
            pHp.changeHealth(-collisionDamage);
            pC.AttackBack(gameObject,attackFroce,stunTime);
        }
    }
}
