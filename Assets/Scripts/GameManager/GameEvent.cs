using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public static class GameEvent
{
    public static event Action<int, int> PlayerHealthChange;
    public static event Action<int, int> EnemyHealthChange;
    public static void TriggerPlayerHealthChange(int current,int max)
    {
        PlayerHealthChange?.Invoke(current,max);
    }
    public static void TriggerEnemyHealthChange(int current,int max)
    {
        EnemyHealthChange?.Invoke(current, max);
    }
}
