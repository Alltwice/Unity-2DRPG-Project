using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
/// <summary>
/// �������״̬�Լ����һЩ��ҹ��ܺ���
/// </summary>
public class PlayerController : MonoBehaviour
{
    //�������
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public InputManger inputActions;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerDefence defence;
    [HideInInspector] public PlayerDodge dodge;
    [SerializeField] private PlayerBaseDataSO BaseData;
    public PlayerBaseDataSO BaseDataSO => BaseData;
    //����״̬
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
        GameEvent.ItemUsed += OnItemUse;
        GameEvent.ItemDropped += OnItemDrop;
    }
    private void OnDisable()
    {
        GameEvent.PlayerHited -= ChangeStateUnderAttack;
        GameEvent.ItemUsed -= OnItemUse;
        GameEvent.ItemDropped -= OnItemDrop;
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
    /// ��������л����״̬
    /// </summary>
    public void ChangeState(PlayerStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();

    }
    /// <summary>
    /// �ܻ�ʱ������Ƿ�ֶܴ����¼��л�����ͬ���ܻ�״̬
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
    public void OnItemUse(ItemInstance currentUse, int index)
    {
        if (currentUse == null || currentUse.definition == null)
            return;

        currentUse.definition.UseMethod(this.gameObject, index);
    }

    public void OnItemDrop(ItemInstance currentDrop, int index)
    {
        if (currentDrop == null || currentDrop.definition == null)
            return;
        if (index < 0 || index >= InventoryManager.Instance.slots.Count)
            return;

        InventoryManager.Instance.RemoveItem(index, 1);
    }
}   
