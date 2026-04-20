using System;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySaveIdentity : MonoBehaviour
{
    [SerializeField] private string enemyId;
    public string EnemyId => enemyId;

    [ContextMenu("Regenerate Enemy Id")]
    public void RegenerateEnemyId()
    {
        enemyId = Guid.NewGuid().ToString("N");
    }

    private void Reset()
    {
        if (string.IsNullOrWhiteSpace(enemyId))
            RegenerateEnemyId();
    }

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(enemyId))
            RegenerateEnemyId();
    }
}
