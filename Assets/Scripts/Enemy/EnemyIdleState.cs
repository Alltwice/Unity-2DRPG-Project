using UnityEngine;
using UnityEngine.UIElements;

public class EnemyIdleState : EnemyStateMachine
{
    protected EnemyTorch torch;
    public EnemyIdleState(EnemyTorch torch)
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
        am.SetBool("isIdle", true);
    }
    public override void OnUpdate()
    {
        torch.CheakPlayer();
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        am.SetBool("isIdle", false);
    }
}
