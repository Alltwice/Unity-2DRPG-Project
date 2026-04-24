using NUnit.Framework;
using UnityEngine;

public class PlayerMoveState:PlayerStateMachine
{
    protected Animator am;
    protected Rigidbody2D rg;
    protected PlayerController playerWarrior;
    protected PlayerCombat combat;
    protected PlayerDefence defence;
    protected PlayerDodge dodge;
    private int faceDirection=1;
    private TestPlayerStatModifiers statModifiers;
    public Vector2 lastMoveDirection = Vector2.right;
    private Vector2 input;
    public PlayerMoveState(PlayerController playerWarrior)
    {
        this.playerWarrior = playerWarrior;
        defence = playerWarrior.defence;
        combat = playerWarrior.combat;
        dodge = playerWarrior.dodge;
        am = playerWarrior.am;
        rg = playerWarrior.rg;
        statModifiers = playerWarrior.GetComponent<TestPlayerStatModifiers>();
        if (statModifiers == null)
            statModifiers = playerWarrior.gameObject.AddComponent<TestPlayerStatModifiers>();
    }

    public override void OnEnter()
    {
        am.SetBool("isMoving", true);
    }

    public override void OnExit()
    {
        am.SetBool("isMoving", false);
    }

    public override void OnFixedUpdate()
    {
        float effectiveSpeed = playerWarrior.BaseDataSO.MoveSpeed + (statModifiers != null ? statModifiers.MoveSpeedBonus : 0f);
        rg.linearVelocity = InputManger.Instance.moveInput * Mathf.Max(0f, effectiveSpeed);
        if (InputManger.Instance.moveInput.x > 0 && playerWarrior.transform.localScale.x < 0 || InputManger.Instance.moveInput.x < 0 && playerWarrior.transform.localScale.x > 0)
        {
            faceDirection *= -1;
            playerWarrior.transform.localScale=new Vector3(faceDirection*playerWarrior.transform.localScale.x,playerWarrior.transform.localScale.y,playerWarrior.transform.localScale.z);
        }
    }

    public override void OnUpdate()
    {
        input = InputManger.Instance.moveInput;
        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = input.normalized;
        }
        if (InputManger.Instance.moveInput==Vector2.zero)
        {
            playerWarrior.ChangeState(playerWarrior.idleState);
        }
        else if (combat.HaveAttackBuffer() == true)
        {
            combat.attackBufferTimer = 0;
            playerWarrior.ChangeState(playerWarrior.attackState);
        }
        else if (defence.isBlocking == true)
        {
            playerWarrior.ChangeState(playerWarrior.defenceStage);
        }
        else if(dodge.HaveBufferTime()&&dodge.rollColdDown<=0)
        {
            dodge.inputBufferTime = 0;
            dodge.StartRool();
        }
    }
}
