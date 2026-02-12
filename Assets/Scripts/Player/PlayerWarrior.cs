using System.Collections;
using UnityEngine;

public class PlayerWarrior : MonoBehaviour
{
    //基础数据
    public float moveSpeed;
    [HideInInspector] public bool isbacking=false;
    //需求组件
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public InputManager inputActions;
    //需求状态
    protected PlayerStateMachine currentState;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerUnderAttackState underAttackState;
    private void Awake()
    {
        am = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        inputActions = GetComponent<InputManager>();
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        underAttackState = new PlayerUnderAttackState(this);
    }
    void Start()
    {
        changeState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdate();
    }
    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }
    public void changeState(PlayerStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();

    }
}
