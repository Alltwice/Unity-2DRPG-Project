using UnityEngine;

public class PlayerIdleState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    public PlayerIdleState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
    }
    public override void OnEnter()
    {
        Debug.Log("现在是待机");
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        rg.linearVelocity = Vector2.zero;
        InputManger.AttackEvent += HandleAttack;
    }

    public override void OnExit()
    {
        InputManger.AttackEvent -= HandleAttack;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        if (InputManger.Instance.moveInput!= Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.moveState);
        }
    }
    public void HandleAttack()
    {
        playerWarrior.ChangeState(playerWarrior.attackState);
    }
}
