using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerWarrior : MonoBehaviour
{
    //需求组件
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public InputManger inputActions;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerCombat combat;
    //攻击相关
    public Transform attackPoint;
    public Vector2 attackRange;
    public LayerMask attackLayer;
    Collider2D[] hits = new Collider2D[10];
    //需求状态
    protected PlayerStateMachine currentState;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerUnderAttackState underAttackState;
    public PlayerAttackState attackState;
    public PlayerDefenceStage defenceStage;
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
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        underAttackState = new PlayerUnderAttackState(this);
        attackState = new PlayerAttackState(this);
        defenceStage = new PlayerDefenceStage(this);
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
    public void ChangeState(PlayerStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();

    }
    public void ChangeStateIdle()
    {
        currentState?.OnExit();
        currentState = idleState;
        currentState?.OnEnter();
    }
    public void ChangeStateUnderAttack()
    {
        currentState?.OnExit();
        currentState = underAttackState;
        currentState?.OnEnter();
    }
    public void Attack()
    {
        hits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f, attackLayer);
        if(hits.Length==0)
        {
            return;
        }
        else if(hits.Length>0)
        {
            for(int i=0;i<hits.Length;i++)
            {
                hits[i].GetComponent<EnemyHealth>()?.ChangeHealth(PlayerDateManger.instance.damage, transform.position);
            }
        }
    }
}
