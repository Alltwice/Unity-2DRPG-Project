using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameEvent
{
    public enum SFXType
    {
        PlayerBeHit,
        EnemyBeHit,
        SwordSwing,
        PlayerDefenceBeHit,
        ClickUI
    }
    public static event Action<int, int> PlayerHealthChange;
    public static event Action<int, int> PlayerHealthSyncOnly;
    public static event Action PlayerDeath;
    public static event Action PlayerHited;
    public static event Action InventoryChanged;
    public static event Action<SFXType> PlaySFX;
    public static event Action<float> CameraShake;
    public static event Action<ItemInstance,Vector3,int> InventoryClicked;
    public static event Action<ItemInstance,int> ItemUsed;
    public static event Action<ItemInstance, int> ItemEquipped;
    public static event Action<ItemInstance, int> ItemDropped;
    public static event Action BagClose;
    public static void TriggerPlayerHealthChange(int current,int max)=>PlayerHealthChange?.Invoke(current,max);
    public static void TriggerPlayerHealthSyncOnly(int current, int max) => PlayerHealthSyncOnly?.Invoke(current, max);
    public static void TriggerPlayerDeath()=>PlayerDeath?.Invoke();
    public static void TriggerPlayerHited()=>PlayerHited?.Invoke();
    public static void TriggerInventoryChanged()=>InventoryChanged?.Invoke();
    public static void TriggerCameraShake(float force)=>CameraShake?.Invoke(force);
    public static void TriggerPlaySFX(SFXType sFX)=>PlaySFX?.Invoke(sFX);
    public static void TriggerClickUISfx() => TriggerPlaySFX(SFXType.ClickUI);
    public static void TriggerInventoryClicked(ItemInstance item,Vector3 transform,int Index)=>InventoryClicked?.Invoke(item,transform,Index);
    public static void TriggerItemUsed(ItemInstance item,int index) => ItemUsed?.Invoke(item,index);
    public static void TriggerItemEquipped(ItemInstance item, int index) => ItemEquipped?.Invoke(item, index);
    public static void TriggerItemDropped(ItemInstance item, int index) => ItemDropped?.Invoke(item, index);
    public static void TriggerBagClose() => BagClose?.Invoke();
}
