using UnityEngine;

public class InputManger : MonoBehaviour
{
    private PlayerAction input;
    public UIManger uIManger;

    public Vector2 moveInput;
    public bool isAttack;
    public bool isPause;

    private void Awake()
    {
        input = new PlayerAction();
        uIManger = GameObject.Find("UIManger").GetComponent<UIManger>();

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
        input.Player.Pause.performed += ctx =>
        {
            isPause = true;
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
