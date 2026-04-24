using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHurtState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected EnemyTorch torch;
    public float stunTime = 0.25f;
    public float beatBackForce=15f;
    public float speedDown = 5f;
    public Vector2 damageSource;
    public Vector2 beatBackDirection;
    public PlayerHurtState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        am.Play("HeroKnight_Hurt");
        playerWarrior.combat.comboStep = 0;
        stunTime = 0.25f;
        damageSource = playerWarrior.playerHealth.attackObject;
        beatBackDirection = ((Vector2)playerWarrior.transform.position - damageSource).normalized;
        playerWarrior.rg.linearVelocity = Vector2.zero;
        playerWarrior.rg.linearVelocity = beatBackForce * beatBackDirection;
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
        playerWarrior.rg.linearVelocity = Vector2.Lerp(playerWarrior.rg.linearVelocity, Vector2.zero, Time.deltaTime * speedDown);
        if(stunTime<=0)
        {
            playerWarrior.ChangeState(playerWarrior.idleState);
        }
    }

}
