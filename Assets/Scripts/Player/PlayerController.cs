using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
/// <summary>
/// 处理玩家状态以及存放一些玩家功能函数
/// </summary>
public class PlayerController : MonoBehaviour
{
    //需求组件
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public InputManger inputActions;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerDefence defence;
    [HideInInspector] public PlayerDoge doge;
    //攻击相关
    public Transform attackPoint;
    public Vector2 attackRange;
    public LayerMask attackLayer;
    Collider2D[] hits = new Collider2D[10];
    //需求状态
    protected PlayerStateMachine currentState;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerHurtState hurtState;
    public PlayerAttackState attackState;
    public PlayerDefenceState defenceStage;
    public PlayerBlockHitState blockHitState;
    public PlayerRollState rollState;
    //范围可视化
    private void OnDrawGizmosSelected()
    {
        if(attackPoint!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attackRange);
        }
    }
    private void OnEnable()
    {
        GameEvent.PlayerHited += ChangeStateUnderAttack;
    }
    private void OnDisable()
    {
        GameEvent.PlayerHited -= ChangeStateUnderAttack;
    }
    private void Awake()
    {
        am = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        inputActions = GetComponent<InputManger>();
        sr = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        combat = GetComponent<PlayerCombat>();
        defence = GetComponent<PlayerDefence>();
        doge = GetComponent<PlayerDoge>();
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        hurtState = new PlayerHurtState(this);
        attackState = new PlayerAttackState(this);
        defenceStage = new PlayerDefenceState(this);
        blockHitState = new PlayerBlockHitState(this);
        rollState = new PlayerRollState(this);
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
    /// <summary>
    /// 传入参数切换玩家状态
    /// </summary>
    public void ChangeState(PlayerStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();

    }
    /// <summary>
    /// 受击时会根据是否持盾触发事件切换到不同的受击状态
    /// </summary>
    public void ChangeStateUnderAttack()
    {
        if(defence.isBlocking==false)
        {
            currentState?.OnExit();
            currentState = hurtState;
            currentState?.OnEnter();
        }
        else
        {
            currentState?.OnExit();
            currentState = blockHitState;
            currentState?.OnEnter();
        }
    }
    /// <summary>
    /// 在动画事件中调用
    /// </summary>
    public void Attack()
    {
        hits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f, attackLayer);
        if(hits.Length==0)
        {
            return;
        }
        if (hits.Length > 0)
        {
            for(int i=0;i<hits.Length;i++)
            {
                hits[i].GetComponent<EnemyHealth>()?.ChangeHealth(PlayerDateManger.instance.damage, transform.position);
            }
        }
    }
}
