using System.Diagnostics;
using UnityEngine;

public class EnemyDieState : EnemyStateMachine
{
    protected EnemyTorch torch;
    public EnemyDieState(EnemyTorch torch)
    {
        this.torch = torch;
        am = torch.am;
        rg = torch.rg;
    }

    protected Animator am;
    protected Rigidbody2D rg;

    public override void OnEnter()
    {
        rg.linearVelocity = Vector2.zero;
        am.SetBool("isDie", true);

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
