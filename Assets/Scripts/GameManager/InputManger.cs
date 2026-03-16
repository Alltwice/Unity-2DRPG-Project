using System;
using UnityEngine;

public class InputManger : MonoBehaviour
{
    public PlayerAction input;
    public Vector2 moveInput;
    public static event Action AttackEvent;
    public static event Action PauseEvent;
    public static event Action PushDefenceEvent;
    public static event Action CanceldDefenceEvent;
    public static InputManger Instance { get; private set; }

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        input = new PlayerAction();
        Instance = this;
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Attack.performed += _ =>AttackEvent?.Invoke();
        input.Player.Pause.performed += _ => PauseEvent?.Invoke();
        input.Player.Defence.performed += _ => PushDefenceEvent?.Invoke();
        input.Player.Defence.canceled += _ => CanceldDefenceEvent?.Invoke();
    }
    public void OnEnable()
    {
        input?.Enable();
    }

    public void OnDisable()
    {
        input?.Disable();
    }
}
