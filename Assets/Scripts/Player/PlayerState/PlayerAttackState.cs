using UnityEngine;

public class PlayerAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected PlayerCombat combat;
    public PlayerAttackState(PlayerWarrior playerWarrior)
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

    }
}
