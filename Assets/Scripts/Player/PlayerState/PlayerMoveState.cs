using NUnit.Framework;
using UnityEngine;

public class PlayerMoveState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected PlayerCombat combat;
    protected PlayerDefence defence;
    protected PlayerDodge doge;
    private int faceDirection=1;
    public PlayerMoveState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        defence = playerWarrior.defence;
        combat = playerWarrior.combat;
        doge = playerWarrior.dodge;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        am.SetBool("isMoving", true);
    }

    public override void OnExit()
    {
        am.SetBool("isMoving", false);
    }

    public override void OnFixedUpdate()
    {
        //这里！！
        rg.linearVelocity = InputManger.Instance.moveInput * 1;
        if (InputManger.Instance.moveInput.x > 0 && playerWarrior.transform.localScale.x < 0 || InputManger.Instance.moveInput.x < 0 && playerWarrior.transform.localScale.x > 0)
        {
            faceDirection *= -1;
            playerWarrior.transform.localScale=new Vector3(faceDirection*playerWarrior.transform.localScale.x,playerWarrior.transform.localScale.y,playerWarrior.transform.localScale.z);
        }
    }

    public override void OnUpdate()
    {
        if (InputManger.Instance.moveInput==Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.idleState);
        }
        else if (combat.HaveAttackBuffer() == true)
        {
            combat.attackBufferTimer = 0;
            playerWarrior.ChangeState(playerWarrior.attackState);
        }
        else if (defence.isBlocking == true)
        {
            playerWarrior.ChangeState(playerWarrior.defenceStage);
        }
        else if(doge.HaveBufferTime()&&doge.coldDown<=0)
        {
            doge.inputBufferTime = 0;
            doge.StartRool();
        }
    }
}
