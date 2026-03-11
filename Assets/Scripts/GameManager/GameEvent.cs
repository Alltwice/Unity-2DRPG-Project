using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public static class GameEvent
{
    public static event Action<int, int> PlayerHealthChange;
    public static void TriggerPlayerHealthChange(int current,int max)
    {
        PlayerHealthChange?.Invoke(current,max);
    }
}
