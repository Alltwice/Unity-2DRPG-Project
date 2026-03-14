using DG.Tweening;
using UnityEngine;

public class EnemyUnderAttack : EnemyStateMachine
{
    protected EnemyTorch torch;
    public float stunTime=0.25f;
    public float beatBackForce = 10f;
    public float speedDown = 10f;
    public Vector2 damageSource;
    public Vector2 beatBackDirection;
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
        stunTime = 0.25f;
        damageSource = torch.enemyHealth.attackObject;
        beatBackDirection= ((Vector2)torch.transform.position-damageSource).normalized;
        rg.linearVelocity = Vector2.zero;
        rg.linearVelocity = beatBackForce * beatBackDirection;
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
        rg.linearVelocity = Vector2.Lerp(rg.linearVelocity, Vector2.zero, Time.deltaTime * speedDown);
        if (stunTime<=0)
        {
            torch.ChangeState(torch.idleState);
        }
    }
}
