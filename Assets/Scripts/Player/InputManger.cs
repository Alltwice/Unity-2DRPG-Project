using UnityEngine;

public class InputManger : MonoBehaviour
{
    private PlayerAction input;

    public Vector2 moveInput;
    public bool isAttack;

    private void Awake()
    {
        input = new PlayerAction();

        input.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        input.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };
        input.Player.Attack.performed += ctx =>
        {
            isAttack = true;
        };
    }
    public void OnEnable()
    {
        input.Enable();
    }

    public void OnDisable()
    {
        input.Disable();
    }
}
