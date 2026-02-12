using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUnderAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected InputManager inputActions;
    protected EnemyTorch torch;
    public PlayerUnderAttackState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
    }
    public override void OnEnter()
    {
        Debug.Log("现在是受击");
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        inputActions = playerWarrior.inputActions;
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
