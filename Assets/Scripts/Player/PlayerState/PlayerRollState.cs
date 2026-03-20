using UnityEngine;

public class PlayerRollState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected PlayerDodge doge;
    public PlayerRollState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        doge = playerWarrior.dodge;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        am.Play("HeroKnight_Roll");
    }

    public override void OnExit()
    {
        rg.linearVelocity = Vector2.zero;
    }

    public override void OnFixedUpdate()
    {
        rg.linearVelocity = doge.lastMoveDirection * doge.BaseData.RollSpeed;
    }

    public override void OnUpdate()
    {

    }
}
