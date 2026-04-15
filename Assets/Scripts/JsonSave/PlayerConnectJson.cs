using UnityEngine;

/// <summary>
/// 负责玩家状态和 JSON 数据的转换。
/// </summary>
public static class PlayerConnectJson
{
    public static PlayerSnapshotDto BuildFromPlayer(GameObject playerObj)
    {
        if (playerObj == null)
            return null;

        var snapshot = new PlayerSnapshotDto();
        Transform tf = playerObj.transform;
        snapshot.posX = tf.position.x;
        snapshot.posY = tf.position.y;

        PlayerHealth health = playerObj.GetComponent<PlayerHealth>();
        if (health != null)
            snapshot.currentHealth = health.currentHealth;

        SpriteRenderer sr = playerObj.GetComponent<SpriteRenderer>();
        if (sr != null)
            snapshot.facingX = sr.flipX ? -1f : 1f;
        else
            snapshot.facingX = tf.localScale.x >= 0f ? 1f : -1f;

        return snapshot;
    }

    public static void ApplyToPlayer(GameData data, GameObject playerObj)
    {
        if (data == null || data.player == null || playerObj == null)
            return;

        PlayerSnapshotDto snapshot = data.player;
        Transform tf = playerObj.transform;
        tf.position = new Vector3(snapshot.posX, snapshot.posY, tf.position.z);

        float normalizedFacing = snapshot.facingX >= 0f ? 1f : -1f;
        SpriteRenderer sr = playerObj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = normalizedFacing < 0f;

        Vector3 localScale = tf.localScale;
        localScale.x = Mathf.Abs(localScale.x) * normalizedFacing;
        tf.localScale = localScale;

        PlayerHealth health = playerObj.GetComponent<PlayerHealth>();
        if (health == null)
            return;

        health.SyncHealthOnly(snapshot.currentHealth);
    }
}
