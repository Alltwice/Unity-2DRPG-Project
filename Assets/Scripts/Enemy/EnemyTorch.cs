using UnityEngine;

public class EnemyTorch : MonoBehaviour
{
    [HideInInspector] public Animator am;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public GameObject player;

    private EnemyStateMachine currentState;
    private EnemyIdleState idleState;

    private void Awake()
    {
        am = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Warrior_Player");
    }
    void Start()
    {
        ChangeState(idleState);
        currentState.OnEnter();
    }

    void Update()
    {
        
    }
    private void ChangeState(EnemyStateMachine newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
