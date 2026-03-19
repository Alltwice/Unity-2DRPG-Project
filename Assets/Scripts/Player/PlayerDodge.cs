using TMPro;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public float inputBufferTime;
    public float rollSpeed;
    public bool isRoll=false;
    public PlayerController player;
    public Vector2 lastMoveDirection = Vector2.right;
    public Vector2 input;
    public float coldDown;
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
        if(coldDown>0)
        {
            coldDown -= Time.deltaTime;
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
        coldDown = 0.5f;
        isRoll = false;
        player.rg.linearVelocity = Vector2.zero;
        player.ChangeState(player.idleState);
    }
}
