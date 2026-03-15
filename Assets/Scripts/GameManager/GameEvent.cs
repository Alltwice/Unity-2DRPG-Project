using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public static class GameEvent
{
    public static event Action<int, int> PlayerHealthChange;
    public static event Action PlayerDeath;
    public static event Action PlayerHited;
    public static event Action<SFXType> PlaySFX;
    public static event Action<float> CameraShake;
    public enum SFXType
    {
        PlayerHit,
        EnemyHit,
        SwordSwing
    }
    public static void TriggerPlayerHealthChange(int current,int max)
    {
        PlayerHealthChange?.Invoke(current,max);
    }
    public static void TriggerPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
    public static void TriggerPlayerHited()
    {
        PlayerHited?.Invoke();
    }
    public static void TriggerCameraShake(float force)
    {
        CameraShake?.Invoke(force);
    }
    public static void TriggerPlaySFX(SFXType sFX)
    {
        PlaySFX?.Invoke(sFX);
    }
}
