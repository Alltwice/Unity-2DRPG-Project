using UnityEngine;

public class EnemyIdleState : EnemyStateMachine
{
    protected EnemyTorch torch;
    public EnemyIdleState(EnemyTorch torch)
    {
        this.torch = torch;
    }
    protected Animator am;
    protected Rigidbody2D rg;
    protected GameObject player;
    public override void OnEnter()
    {
        am = torch.am;
        rg = torch.rg;
        player = torch.player;
        rg.linearVelocity = Vector2.zero;
        am.SetBool("isIdle", true);
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
