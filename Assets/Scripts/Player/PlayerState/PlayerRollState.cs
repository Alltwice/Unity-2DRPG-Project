using UnityEngine;

public class PlayerRollState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    public PlayerRollState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        am.Play("Warrior_Idle");
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
        if (InputManger.Instance.moveInput != Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.moveState);
        }
    }
}
