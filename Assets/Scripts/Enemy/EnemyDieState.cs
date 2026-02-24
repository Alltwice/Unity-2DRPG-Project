using System.Diagnostics;
using UnityEngine;

public class EnemyDieState : EnemyStateMachine
{
    protected EnemyTorch torch;
    public EnemyDieState(EnemyTorch torch)
    {
        this.torch = torch;
    }

    protected Animator am;
    protected Rigidbody2D rg;

    public override void OnEnter()
    {
        UnityEngine.Debug.Log("真死了");
        am = torch.am;
        rg = torch.rg;
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
