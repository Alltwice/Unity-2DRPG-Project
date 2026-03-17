using UnityEngine;

public class PlayerDefenceState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    public PlayerDefenceState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        playerWarrior.combat.comboStep = 0;
        am.Play("HeroKnight_IdleBlock");
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

    }
}
