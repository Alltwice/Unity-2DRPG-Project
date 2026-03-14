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
    //需求状态
    protected PlayerStateMachine currentState;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerUnderAttackState underAttackState;
    public PlayerAttackState attackState;
    //范围可视化
    private void OnDrawGizmosSelected()
    {
        if(PlayerDateManger.instance.attackPoint!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(PlayerDateManger.instance.attackPoint.position, PlayerDateManger.instance.attackRange);
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
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        underAttackState = new PlayerUnderAttackState(this);
        attackState = new PlayerAttackState(this);
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
        Collider2D[] hits = Physics2D.OverlapBoxAll(PlayerDateManger.instance.attackPoint.position, PlayerDateManger.instance.attackRange, 0f, PlayerDateManger.instance.attackLayer);
        if(hits.Length>0)
        {
            hits[0].GetComponent<EnemyHealth>().ChangeHealth(PlayerDateManger.instance.damage,transform.position);
        }
    }
}
