using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责敌人与 JSON 数据互转。
/// </summary>
public static class EnemyConnectJson
{
    public static List<EnemySnapshotDto> BuildFromEnemies()
    {
        var snapshots = new List<EnemySnapshotDto>();
        EnemySaveIdentity[] identities = Object.FindObjectsByType<EnemySaveIdentity>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (EnemySaveIdentity identity in identities)
        {
            if (identity == null || !identity.gameObject.scene.IsValid())
                continue;

            if (string.IsNullOrWhiteSpace(identity.EnemyId))
            {
                Debug.LogWarning($"EnemyConnectJson: 敌人 {identity.gameObject.name} 缺少 EnemyId，跳过存档。");
                continue;
            }

            GameObject enemyObj = identity.gameObject;
            Transform tf = enemyObj.transform;
            EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();

            var dto = new EnemySnapshotDto
            {
                enemyId = identity.EnemyId,
                posX = tf.position.x,
                posY = tf.position.y,
                currentHealth = health != null ? health.currentHealth : 0,
                isActive = enemyObj.activeSelf,
                isDead = health != null && health.isDie
            };
            snapshots.Add(dto);
        }

        return snapshots;
    }

    public static void ApplyToEnemies(GameData data)
    {
        if (data == null || data.enemies == null || data.enemies.Count == 0)
            return;

        EnemySaveIdentity[] identities = Object.FindObjectsByType<EnemySaveIdentity>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var map = new Dictionary<string, EnemySaveIdentity>();

        foreach (EnemySaveIdentity identity in identities)
        {
            if (identity == null || !identity.gameObject.scene.IsValid())
                continue;
            if (string.IsNullOrWhiteSpace(identity.EnemyId))
                continue;

            if (!map.ContainsKey(identity.EnemyId))
                map.Add(identity.EnemyId, identity);
            else
                Debug.LogWarning($"EnemyConnectJson: 发现重复 EnemyId={identity.EnemyId}，可能导致读档映射不稳定。");
        }

        foreach (EnemySnapshotDto snapshot in data.enemies)
        {
            if (snapshot == null || string.IsNullOrWhiteSpace(snapshot.enemyId))
                continue;

            if (!map.TryGetValue(snapshot.enemyId, out EnemySaveIdentity identity))
            {
                Debug.LogWarning($"EnemyConnectJson: 当前场景未找到 EnemyId={snapshot.enemyId}，跳过该敌人还原。");
                continue;
            }

            GameObject enemyObj = identity.gameObject;
            enemyObj.SetActive(true);

            Transform tf = enemyObj.transform;
            tf.position = new Vector3(snapshot.posX, snapshot.posY, tf.position.z);

            EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
            if (health != null)
                health.ApplySnapshot(snapshot.currentHealth, snapshot.isDead);

            enemyObj.SetActive(snapshot.isActive);
        }
    }
}
