using System;
using UnityEngine;

public class InputManger : MonoBehaviour
{
    public PlayerAction input;
    public Vector2 moveInput;
    [Header("Spawn Test Config")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private GameObject[] randomItemPrefabs;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private float itemRandomRadius = 1f;

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
        input.Player.SpawnEnemy.performed += _ => TestSpawnEnemy();
        input.Player.SpawnItem.performed += _ => TestSpawnItem();
    }
    public void OnEnable()
    {
        input?.Enable();
    }

    public void OnDisable()
    {
        input?.Disable();
    }

    private void TestSpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("InputManger: enemyPrefab is not assigned.");
            return;
        }

        Vector3 spawnPos = enemySpawnPoint != null ? enemySpawnPoint.position : transform.position;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    private void TestSpawnItem()
    {
        if (randomItemPrefabs == null || randomItemPrefabs.Length == 0)
        {
            Debug.LogWarning("InputManger: randomItemPrefabs is empty.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, randomItemPrefabs.Length);
        GameObject randomItemPrefab = randomItemPrefabs[randomIndex];
        if (randomItemPrefab == null)
        {
            Debug.LogWarning("InputManger: selected item prefab is null.");
            return;
        }

        Vector3 basePos = itemSpawnPoint != null ? itemSpawnPoint.position : transform.position;
        Vector2 offset = UnityEngine.Random.insideUnitCircle * Mathf.Max(0f, itemRandomRadius);
        Vector3 spawnPos = new Vector3(basePos.x + offset.x, basePos.y + offset.y, basePos.z);
        Instantiate(randomItemPrefab, spawnPos, Quaternion.identity);
    }
}
