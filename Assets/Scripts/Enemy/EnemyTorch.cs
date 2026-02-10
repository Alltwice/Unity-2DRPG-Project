using UnityEngine;

public class EnemyTorch : MonoBehaviour
{
    //基础属性
    public float moveSpeed;
    public float attackDetectRange;
    public Vector2 attackRange;
    public int damage;
    public float attackFroce;
    public float stunTime;
    public Transform attackPoint;
    //需求组件
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public GameObject player;
    //感知范围
    public Transform detectPoint;
    public float detectRange;
    public LayerMask detectLayer;
    //状态
    private EnemyStateMachine currentState;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyChaseState chaseState;
    [HideInInspector] public EnemyCombatState combatState;

    //感知范围显示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectPoint.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position,attackRange);
    }

    //感知能力
    public void CheakPlayer()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(detectPoint.position, detectRange, detectLayer);
        if (hit.Length > 0)
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= attackDetectRange)
            {
                rg.linearVelocity = Vector2.zero;
                ChangeState(combatState);
            }
            else if (Vector2.Distance(player.transform.position, transform.position) > attackDetectRange)
            {
                ChangeState(chaseState);
            }
        }
        else
        {
            ChangeState(idleState);
        }
    }
    //攻击能力
    public void Attack()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, attackRange, detectLayer);
        if (hit.Length > 0)
        {
            player.GetComponent<PlayerHealth>().changeHealth(damage);
            player.GetComponent<PlayerController>().AttackBack(gameObject, attackFroce, stunTime);
        }
    }
    private void Awake()
    {
        am = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Warrior_Player");
        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        combatState = new EnemyCombatState(this);
    }
    void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }
    public void ChangeState(EnemyStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }
}
