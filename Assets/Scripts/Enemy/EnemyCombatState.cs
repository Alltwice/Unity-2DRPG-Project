using UnityEngine;

public class EnemyCombatState : EnemyStateMachine
{
    protected EnemyTorch torch;
    public EnemyCombatState(EnemyTorch torch)
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
        am.SetBool("isAttack", true);
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
        am.SetBool("isAttack", false);
    }
}
