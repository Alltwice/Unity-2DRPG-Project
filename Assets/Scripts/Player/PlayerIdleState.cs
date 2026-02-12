using UnityEngine;

public class PlayerIdleState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManager inputActions;
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
        if (playerWarrior.isbacking == true)
        {
            return;
        }
        if (inputActions.MoveInput!=Vector2.zero)
        {
            playerWarrior.changeState(playerWarrior.moveState);
        }
    }
}
