using UnityEngine;

public class PlayerIdleState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManger inputActions;
    public PlayerIdleState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
    }
    public override void OnEnter()
    {
        Debug.Log("现在是待机");
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        inputActions = playerWarrior.inputActions;
        rg.linearVelocity = Vector2.zero;
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        if (inputActions.moveInput!=Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.moveState);
        }
        if(inputActions.isAttack==true)
        {
            playerWarrior.ChangeState(playerWarrior.attackState);
        }
    }
}
