using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBlockHitState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected EnemyTorch torch;
    public float stunTime = 0.15f;
    public float beatBackForce = 6f;
    public float speedDown = 10f;
    public Vector2 damageSource;
    public Vector2 beatBackDirection;
    public PlayerBlockHitState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        playerWarrior.combat.comboStep = 0;
        stunTime = 0.15f;
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
        if (stunTime <= 0)
        {
            playerWarrior.ChangeState(playerWarrior.defenceStage);
        }
    }

}

