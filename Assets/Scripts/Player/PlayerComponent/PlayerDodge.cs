using TMPro;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [SerializeField] private PlayerBaseDataSO baseData;

    public float inputBufferTime;
    public bool isRoll = false;
    private PlayerController player;
    public float rollColdDown;

    public void OnEnable()
    {
        InputManger.RollEvent += OnDogeInput;
    }
    public void OnDisable()
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
        if (rollColdDown > 0)
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

        player.ChangeState(player.rollState);
    }
    public void EndRoll()
    {
        rollColdDown = baseData.RollColdDown;
        isRoll = false;
        player.rg.linearVelocity = Vector2.zero;
        player.ChangeState(player.idleState);
    }

    // Expose SO for other scripts that previously referenced configuration fields
    public PlayerBaseDataSO BaseData => baseData;
}
