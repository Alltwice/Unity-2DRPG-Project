    using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class PlayerCombat : MonoBehaviour
{
    public PlayerController player;
    private Animator anim;
    public int comboStep = 0;
    public float attackBufferTimer = 0f;
    public bool canInputNextCombo = false;
    //攻击相关
    private int damage;
    public Transform attackPoint;
    private Vector2 attackRange;
    private LayerMask attackLayer;
    private Collider2D[] hits = new Collider2D[10];
    //范围可视化
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attackRange);
        }
    }
    public void PlayerCombatInitialize(int damage,Vector2 attackRange,LayerMask attackLayer)
    {
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackLayer = attackLayer;
    }
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 全局监听按键
        InputManger.AttackEvent += OnAttackInput;
    }

    private void OnDisable()
    {
        InputManger.AttackEvent -= OnAttackInput;
    }

    private void OnAttackInput()
    {
        //按下按键时存在0.2秒的缓冲
        attackBufferTimer = 0.2f; 
    }

    private void Update()
    {
        //关于这里可以吧attackBufferTimer理解为一段输入信号，你只要按键了就会变成一段0.2秒的信号，如果超过了0.2秒就什么都没有发生
        //但如果在这0.2秒里检测到可以攻击就会触发这段信号变成攻击行为，包括检测连段
        if (attackBufferTimer > 0)
        {
            //从存在输入缓冲开始时开始计时
            //状态切换的逻辑写在状态机内
            attackBufferTimer -= Time.deltaTime;
        }
    }

    // 由 AttackState 刚刚进入时调用打出第一段
    public void StartAttack()
    {
        comboStep = 1;
        anim.Play("Warrior_Attack1");
        //等待动画事件判断是否可以进入下一段攻击
        canInputNextCombo = false;
    }

    // 执行后续连段
    public void ExecuteCombo()
    {
        canInputNextCombo = false;
        comboStep++;
        if (comboStep > 4)
        {
            comboStep = 1;
        }
        anim.Play("Warrior_Attack" + comboStep);
    }

    // --- 动画事件 ---
    public void OpenComboWindow()
    {
        canInputNextCombo = true; 
    }

    public void EndAttack()
    {
        comboStep = 0;
        canInputNextCombo = false;
        player.ChangeState(player.idleState);
    }
    public bool HaveAttackBuffer()
    {
        return attackBufferTimer > 0;
    }
    /// <summary>
    /// 在动画事件中调用
    /// </summary>
    public void Attack()
    {
        hits = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0f, attackLayer);
        if (hits.Length == 0)
        {
            return;
        }
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i].GetComponent<EnemyHealth>()?.ChangeHealth(damage, transform.position);
            }
        }
    }
}

