using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class EnemyTorch : MonoBehaviour
{
    //基础属性
    public float moveSpeed;
    public int damage;
    public float attackFroce;
    public float stunTime;
    //需求组件
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public GameObject player;
    [HideInInspector] public SpriteRenderer sr;
    private EnemyHealth enemyHealth;
    //感知/攻击范围
    public Transform detectPoint;
    public float detectRange;
    public LayerMask detectLayer;
    public Transform attackPoint;
    public Vector2 attackRange;
    public float attackDetectRange;
    public LayerMask attackLayer;
    //状态
    private EnemyStateMachine currentState;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyChaseState chaseState;
    [HideInInspector] public EnemyCombatState combatState;
    [HideInInspector] public EnemyDieState dieState;
    public EnemyUnderAttack underAttack;
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
    private void OnEnable()
    {
        enemyHealth.EnemyHited += ChangeStateUnderAttack;
    }
    private void OnDisable()
    {
        enemyHealth.EnemyHited -= ChangeStateUnderAttack;
    }

    //感知能力
    public void CheakPlayer()
    {
        if(enemyHealth.isDie==true)
        {
            Debug.Log("确认死亡状态");
            ChangeState(dieState);
        }
        else
        {
            Collider2D[] cheak = Physics2D.OverlapCircleAll(detectPoint.position, detectRange, detectLayer);
            if (cheak.Length > 0)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= attackDetectRange)
                {
                    rg.linearVelocity = Vector2.zero;
                    ChangeState(combatState);
                }
                else if (Vector2.Distance(player.transform.position, transform.position) > attackDetectRange && currentState != combatState)
                {
                    ChangeState(chaseState);
                }
            }
            else
            {
                ChangeState(idleState);
            }
        }
    }
    //攻击能力
    public void Attack()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f, attackLayer);
        if (hit.Length > 0)
        {
            player.GetComponent<PlayerHealth>().changeHealth(damage);
        }
    }
    //死亡
    public void Die()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        am = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Warrior_Player");
        enemyHealth = GetComponent<EnemyHealth>();
        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        combatState = new EnemyCombatState(this);
        dieState = new EnemyDieState(this);
        underAttack = new EnemyUnderAttack(this);
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
    public void amEventChangeStateIdle()
    {
        currentState?.OnExit();
        currentState = idleState;
        currentState?.OnEnter();
    }
    public void ChangeStateUnderAttack()
    {
        currentState?.OnExit();
        currentState = underAttack;
        currentState?.OnEnter();
    }
}
