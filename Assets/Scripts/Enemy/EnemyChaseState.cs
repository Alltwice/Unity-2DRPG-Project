using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChaseState : EnemyStateMachine
{
    protected EnemyTorch torch;
    private Vector2 direction;
    public EnemyChaseState(EnemyTorch torch)
    {
        this.torch = torch;
    }
    protected Animator am;
    protected Rigidbody2D rg;
    protected GameObject player;

    private int faceDirection=1;
    public override void OnEnter()
    {
        am = torch.am;
        rg = torch.rg;
        player = torch.player;
        am.SetBool("isChase", true);
    }
    public override void OnUpdate()
    {
        torch.CheakPlayer();
    }
    public override void OnFixedUpdate()
    {
        if (player.transform.position.x < torch.transform.position.x && faceDirection > 0 ||
        player.transform.position.x > torch.transform.position.x && faceDirection < 0)
        {
            faceDirection *= -1;
            torch.transform.localScale = new Vector3(faceDirection, torch.transform.localScale.y, torch.transform.localScale.z);
        }
        direction = (player.transform.position - torch.transform.position).normalized;
        rg.linearVelocity = direction * torch.moveSpeed;
    }

    public override void OnExit()
    {
        am.SetBool("isChase", false);
    }
}
