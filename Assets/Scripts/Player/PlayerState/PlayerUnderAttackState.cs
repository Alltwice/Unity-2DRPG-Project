using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUnderAttackState : PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerWarrior playerWarrior;
    protected EnemyTorch torch;
    public float stunTime = 0.25f;
    public PlayerUnderAttackState(PlayerWarrior playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
    }
    public override void OnEnter()
    {
        //该部分使用了DOTween插件实现简单受击动画
        playerWarrior.sr.DOKill();
        playerWarrior.sr.color = Color.red;
        playerWarrior.sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutQuad);
        playerWarrior.transform.DOShakePosition(0.15f, 0.2f);
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
            playerWarrior.ChangeState(playerWarrior.idleState);
        }
    }
}
