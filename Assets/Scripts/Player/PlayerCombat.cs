using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerWarrior player;
    private Animator anim;
    public int comboStep = 0;
    public float attackBufferTimer = 0f;
    public bool canInputNextCombo = false;

    private void Awake()
    {
        player = GetComponent<PlayerWarrior>();
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
            attackBufferTimer -= Time.deltaTime;
            //当处于没有攻击的状态或是可进行连段的状态消耗缓冲
            if (comboStep == 0 || canInputNextCombo==true)
            {
                attackBufferTimer = 0; // 消耗缓冲

                if (comboStep == 0)
                {
                    //如果是第一刀直接切换到攻击状态
                    player.ChangeState(player.attackState);
                }
                else
                {
                    // 如果是连击直接打下一段
                    ExecuteCombo();
                }
            }
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
    private void ExecuteCombo()
    {
        canInputNextCombo = false;
        comboStep++;
        if (comboStep > 3)
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
        player.ChangeStateIdle();
    }
}

