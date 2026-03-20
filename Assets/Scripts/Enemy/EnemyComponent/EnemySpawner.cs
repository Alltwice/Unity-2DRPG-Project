using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyPool pool;
    private GameObject enemy;
    public Transform spawnPoint;      // 在 Inspector 指定生成点
    public float spawnInterval = 2f;
    private float timer;

    void Start()
    {
        pool = GetComponent<EnemyPool>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        if (pool == null) return;

        enemy = pool.GetEnemy();
        if (enemy != null)
        {
            // 保护检查 spawnPoint
            Vector3 pos = (spawnPoint != null) ? spawnPoint.position : transform.position;
            enemy.transform.position = pos;
            enemy.SetActive(true);
        }
    }
}
