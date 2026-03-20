using TMPro;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public float inputBufferTime;
    public float rollSpeed;
    public bool isRoll=false;
    private PlayerController player;
    public Vector2 lastMoveDirection = Vector2.right;
    private Vector2 input;
    public float rollColdDown;
    private float markColdDown;
    public void PlayerDodgeInitialize(float rollSpeed,float markColdDown)
    {
        this.rollSpeed = rollSpeed;
        this.markColdDown = markColdDown;
    }
    private void OnEnable()
    {
        InputManger.RollEvent += OnDogeInput;
    }
    private void OnDisable()
    {
        InputManger.RollEvent -= OnDogeInput;
    }
    public void OnDogeInput()
    {
        inputBufferTime = 0.2f;
    }
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (inputBufferTime > 0)
        {
            inputBufferTime -= Time.deltaTime;
        }
        if(rollColdDown>0)
        {
            rollColdDown -= Time.deltaTime;
        }
    }
    public bool HaveBufferTime()
    {
        return inputBufferTime > 0;
    }
    public void StartRool()
    {
        isRoll = true;
        input = InputManger.Instance.moveInput;
        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = input.normalized;
        }
        player.ChangeState(player.rollState);
    }
    public void EndRoll()
    {
        rollColdDown = markColdDown;
        isRoll = false;
        player.rg.linearVelocity = Vector2.zero;
        player.ChangeState(player.idleState);
    }
}
