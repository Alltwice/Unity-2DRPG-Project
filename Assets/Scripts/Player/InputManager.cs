using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerAction input;

    public Vector2 MoveInput;

    private void Awake()
    {
        input = new PlayerAction();

        input.Player.Move.performed += ctx =>
        {
            MoveInput = ctx.ReadValue<Vector2>();
        };

        input.Player.Move.canceled += ctx =>
        {
            MoveInput = Vector2.zero;
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
