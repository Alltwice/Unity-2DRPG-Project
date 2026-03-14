using System;
using UnityEngine;

public class InputManger : MonoBehaviour
{
    public PlayerAction input;
    public Vector2 moveInput;
    public static event Action AttackEvent;
    public static event Action PauseEvent;
    public static InputManger Instance { get; private set; }

    private void Awake()
    {
        input = new PlayerAction();
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Attack.performed += _ =>AttackEvent?.Invoke();
        input.Player.Pause.performed += _ => PauseEvent?.Invoke();
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
