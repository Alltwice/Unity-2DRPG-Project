using UnityEngine;

public class PlayerAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    public PlayerAttackState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        am.SetBool("isAttacking", true);
        rg.linearVelocity = Vector2.zero;
    }

    public override void OnExit()
    {
        am.SetBool("isAttacking", false);
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {

    }
}
