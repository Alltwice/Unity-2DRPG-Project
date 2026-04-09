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
    public static event Action<ItemDataSO,Vector3,int> InventoryClicked;
    public static event Action<ItemDataSO,int> ItemUsed;
    public static event Action<ItemDataSO> ItemEquipped;
    public static event Action<ItemDataSO> ItemDropped;
    public static event Action BagClose;
    public enum SFXType
    {
        PlayerBeHit,
        EnemyBeHit,
        SwordSwing,
        PlayerDefenceBeHit
    }
    public static void TriggerPlayerHealthChange(int current,int max)=>PlayerHealthChange?.Invoke(current,max);
    public static void TriggerPlayerDeath()=>PlayerDeath?.Invoke();
    public static void TriggerPlayerHited()=>PlayerHited?.Invoke();
    public static void TriggerInventoryChanged()=>InventoryChanged?.Invoke();
    public static void TriggerCameraShake(float force)=>CameraShake?.Invoke(force);
    public static void TriggerPlaySFX(SFXType sFX)=>PlaySFX?.Invoke(sFX);
    public static void TriggerInventoryClicked(ItemDataSO item,Vector3 transform,int Index)=>InventoryClicked?.Invoke(item,transform,Index);
    public static void TriggerItemUsed(ItemDataSO item,int index) => ItemUsed?.Invoke(item,index);
    public static void TriggerItemEquipped(ItemDataSO item) => ItemEquipped?.Invoke(item);
    public static void TriggerItemDropped(ItemDataSO item) => ItemDropped?.Invoke(item);
    public static void TriggerBagClose() => BagClose?.Invoke();
}
