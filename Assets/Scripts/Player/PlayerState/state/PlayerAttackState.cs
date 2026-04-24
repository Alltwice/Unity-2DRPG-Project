using UnityEngine;

public class PlayerAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected PlayerCombat combat;
    public PlayerAttackState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        combat = playerWarrior.combat;
    }
    public override void OnEnter()
    {
        rg.linearVelocity = Vector2.zero;
        combat.StartAttack();   
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        //当处于没有攻击的状态或是可进行连段的状态消耗缓冲
        if (combat.HaveAttackBuffer() == true && combat.canInputNextCombo == true)
        {
            combat.attackBufferTimer = 0;
            combat.ExecuteCombo();
        }
    }
}
