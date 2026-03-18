using UnityEngine;

public class PlayerIdleState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    public PlayerCombat combat;
    public PlayerDefence defence;
    public PlayerDoge doge;
    public PlayerIdleState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        combat = playerWarrior.combat;
        defence = playerWarrior.defence;
        doge = playerWarrior.doge;
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
        if (InputManger.Instance.moveInput!= Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.moveState);
        }
        else if(combat.HaveAttackBuffer()==true)
        {
            combat.attackBufferTimer = 0;
            combat.StartAttack();
        }
        else if(defence.isBlocking==true)
        {
            playerWarrior.ChangeState(playerWarrior.defenceStage);
        }
        else if (doge.HaveBufferTime()&&doge.coldDown<=0)
        {
            doge.inputBufferTime = 0;
            doge.StartRool();
        }
    }
}
