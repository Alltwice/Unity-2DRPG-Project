using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameEvent
{
    public static event Action<int, int> PlayerHealthChange;
    public static event Action PlayerDeath;
    public static event Action PlayerHited;
    public static event Action InventoryChanged;
    public static event Action<SFXType> PlaySFX;
    public static event Action<float> CameraShake;
    public static event Action<ItemDataSO,Vector3> InventoryClicked;
    public enum SFXType
    {
        PlayerBeHit,
        EnemyBeHit,
        SwordSwing,
        PlayerDefenceBeHit
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
    public static void TriggerInventoryChanged()
    {
        InventoryChanged?.Invoke();
    }
    public static void TriggerCameraShake(float force)
    {
        CameraShake?.Invoke(force);
    }
    public static void TriggerPlaySFX(SFXType sFX)
    {
        PlaySFX?.Invoke(sFX);
    }
    public static void TriggerInventoryClicked(ItemDataSO item,Vector3 transform)
    {
        InventoryClicked?.Invoke(item,transform);
    }
}
