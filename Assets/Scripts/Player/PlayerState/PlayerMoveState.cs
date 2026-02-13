using NUnit.Framework;
using UnityEngine;

public class PlayerMoveState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManger inputActions;
    private int faceDirection=1;
    public PlayerMoveState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
    }
    public override void OnEnter()
    {
        Debug.Log("现在是移动");
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        inputActions = playerWarrior.inputActions;
        am.SetBool("isMoving", true);
    }

    public override void OnExit()
    {
        am.SetBool("isMoving", false);
    }

    public override void OnFixedUpdate()
    {
        rg.linearVelocity = inputActions.moveInput * PlayerDateManger.instance.moveSpeed;
        if (inputActions.moveInput.x > 0 && playerWarrior.transform.localScale.x < 0 || inputActions.moveInput.x < 0 && playerWarrior.transform.localScale.x > 0)
        {
            faceDirection *= -1;
            playerWarrior.transform.localScale=new Vector3(faceDirection*playerWarrior.transform.localScale.x,playerWarrior.transform.localScale.y,playerWarrior.transform.localScale.z);
        }
    }

    public override void OnUpdate()
    {
        if (inputActions.moveInput==Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.idleState);
        }
        if (inputActions.isAttack == true)
        {
            rg.linearVelocity = Vector2.zero;
            playerWarrior.ChangeState(playerWarrior.attackState);
        }
    }
}
