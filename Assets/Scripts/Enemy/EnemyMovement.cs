using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rg;
    private GameObject player;
    private Vector2 direction;
    private float faceDirection;
    private EnemyStage enemyStage = EnemyStage.idle;
    private Animator am;
    private float attackCooldownTimer = 0;

    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public Transform detectPosition;
    public float detectRange = 5;
    public LayerMask detectLayer;
    //敌人状态机
    enum EnemyStage
    {
        idle,
        chase,
        attact
    }
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Warrior_Player");
        am = GetComponent<Animator>();
        enemyStage = EnemyStage.idle;
        faceDirection = gameObject.transform.localScale.x;
    }
    void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        CheakPlayer();
        if (enemyStage == EnemyStage.chase)
        {
            Chase();
        }
    }
    void Chase()
    {
        //在追逐状态中如果进入了攻击范围转换状态，否则继续追逐 
        if (player.transform.position.x < transform.position.x && faceDirection > 0 ||
              player.transform.position.x > transform.position.x && faceDirection < 0)
        {
            faceDirection *= -1;
            transform.localScale = new Vector3(faceDirection, transform.localScale.y, transform.localScale.z);
        }
        direction = (player.transform.position - transform.position).normalized;
        rg.linearVelocity = direction * speed;
    }
    //检测方法，利用OverlapCircle检测范围内玩家并依据条件做出判断
    private void CheakPlayer()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(detectPosition.position, detectRange, detectLayer);
        if (hit.Length > 0)
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= attackRange&& attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                rg.linearVelocity = Vector2.zero;
                ChangeStage(EnemyStage.attact);
            }
            else if (Vector2.Distance(player.transform.position, transform.position) > attackRange&&enemyStage!=EnemyStage.attact)
            {
                ChangeStage(EnemyStage.chase);
            }
        }
        else
        {
            rg.linearVelocity = Vector2.zero;
            ChangeStage(EnemyStage.idle);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPosition.position, detectRange);
    }
    private void ChangeStage(EnemyStage newStage)
    {
        //退出当前状态
        if (enemyStage == EnemyStage.idle)
        {
            am.SetBool("isIdle", false);
        }
        else if (enemyStage == EnemyStage.chase)
        {
            am.SetBool("isChase", false);
        }
        else if (enemyStage == EnemyStage.attact)
        {
            am.SetBool("isAttack", false);
        }
        //更新状态
        enemyStage = newStage;
        //设置状态
        if (enemyStage == EnemyStage.idle)
        {
            am.SetBool("isIdle", true);
        }
        else if (enemyStage == EnemyStage.chase)
        {
            am.SetBool("isChase", true);
        }
        else if (enemyStage == EnemyStage.attact)
        {
            am.SetBool("isAttack", true);
        }
    }
}
