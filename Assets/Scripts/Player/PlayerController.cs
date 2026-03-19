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
    [HideInInspector] public PlayerDodge dodge;
    //需求状态
    protected PlayerStateMachine currentState;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerHurtState hurtState;
    public PlayerAttackState attackState;
    public PlayerDefenceState defenceStage;
    public PlayerBlockHitState blockHitState;
    public PlayerRollState rollState;
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
        dodge = GetComponent<PlayerDodge>();
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
}
