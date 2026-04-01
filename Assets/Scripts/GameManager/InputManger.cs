using System;
using UnityEngine;

public class InputManger : MonoBehaviour
{
    public PlayerAction input;
    public Vector2 moveInput;
    public static event Action AttackEvent;
    public static event Action<PanelType> PauseEvent;
    public static event Action<PanelType> OpenBagEvent;
    public static event Action PushDefenceEvent;
    public static event Action CanceldDefenceEvent;
    public static event Action RollEvent;
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
        input.Player.Move.canceled += _ => moveInput = Vector2.zero;
        input.Player.Attack.performed += _ =>AttackEvent?.Invoke();
        input.Player.Pause.performed += _ => PauseEvent?.Invoke(PanelType.pausePanel);
        input.Player.Defence.performed += _ => PushDefenceEvent?.Invoke();
        input.Player.Defence.canceled += _ => CanceldDefenceEvent?.Invoke();
        input.Player.Roll.performed += _ => RollEvent?.Invoke();
        input.Player.Bag.performed += _ => OpenBagEvent.Invoke(PanelType.bagPanel);
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
