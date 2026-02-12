using NUnit.Framework;
using UnityEngine;

public class PlayerMoveState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManager inputActions;
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
        if(playerWarrior.isbacking==true)
        {
            return;
        }
        rg.linearVelocity = inputActions.MoveInput * playerWarrior.moveSpeed;
        if (inputActions.MoveInput.x > 0 && playerWarrior.transform.localScale.x < 0 || inputActions.MoveInput.x < 0 && playerWarrior.transform.localScale.x > 0)
        {
            faceDirection *= -1;
            playerWarrior.transform.localScale=new Vector3(faceDirection*playerWarrior.transform.localScale.x,playerWarrior.transform.localScale.y,playerWarrior.transform.localScale.z);
        }
    }

    public override void OnUpdate()
    {
        if (playerWarrior.isbacking == true)
        {
            return;
        }
        if (inputActions.MoveInput==Vector2.zero)
        {
            playerWarrior.changeState(playerWarrior.idleState);
        }
    }
}
