using UnityEngine;

public class PlayerAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManger inputActions;
    public PlayerAttackState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
    }
    public override void OnEnter()
    {
        Debug.Log("现在是攻击");
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        inputActions = playerWarrior.inputActions;
        am.SetBool("isAttacking", true);
    }

    public override void OnExit()
    {
        inputActions.isAttack = false;
        am.SetBool("isAttacking", false);
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {

    }
}
