using DG.Tweening;
using UnityEngine;

public class EnemyUnderAttack : EnemyStateMachine
{
    protected EnemyTorch torch;
    public float stunTime=0.25f;
    public EnemyUnderAttack(EnemyTorch torch)
    {
        this.torch = torch;
        am = torch.am;
        rg = torch.rg;
    }

    protected Animator am;
    protected Rigidbody2D rg;
    public override void OnEnter()
    {
        torch.sr.DOKill();
        torch.sr.color = Color.red;
        torch.sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutQuad);
        torch.transform.DOShakePosition(0.15f, 0.3f);
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        stunTime -= Time.deltaTime;
        if(stunTime<=0)
        {
            torch.ChangeState(torch.idleState);
        }
    }
}
