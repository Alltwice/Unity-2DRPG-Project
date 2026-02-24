using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialSize = 20;
    private GameObject tempEnemy;
    private GameObject newEnemy;

    private List<GameObject> enemies;

    private void Awake()
    {
        enemies = new List<GameObject>();
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            tempEnemy = Instantiate(enemyPrefab, transform);
            tempEnemy.SetActive(false);
            enemies.Add(tempEnemy);
        }
    }

    public GameObject GetEnemy()
    {
        // 返回第一个 inactive 的对象
        foreach (GameObject tempEnemy in enemies)
        {
            if (!tempEnemy.activeInHierarchy)
                return tempEnemy;
        }

        // 池耗尽则创建新对象并加入池（默认 inactive）
        newEnemy = Instantiate(enemyPrefab, transform);
        newEnemy.SetActive(false);
        enemies.Add(newEnemy);
        return newEnemy;
    }
}
