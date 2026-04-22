using UnityEngine;

public class TestEquipmentSmokeVerifier : MonoBehaviour
{
    [SerializeField] private GameDataSaveLoad saveLoad;
    [SerializeField] private TestEquipmentManager equipmentManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerController playerController;

    [ContextMenu("Test/打印装备冒烟状态")]
    public void PrintSmokeState()
    {
        if (equipmentManager == null || playerHealth == null || playerController == null)
        {
            Debug.LogWarning("TestEquipmentSmokeVerifier: 请先绑定核心组件引用。");
            return;
        }

        TestPlayerStatModifiers modifiers = playerController.GetComponent<TestPlayerStatModifiers>();
        int damageBonus = modifiers != null ? modifiers.DamageBonus : 0;
        float damageReductionPercent = modifiers != null ? modifiers.DamageReductionPercent : 0f;
        float effectiveMoveSpeed = playerController.BaseDataSO.MoveSpeed + (modifiers != null ? modifiers.MoveSpeedBonus : 0f);
        Debug.Log($"[TestSmoke] MaxHealth={playerHealth.EffectiveMaxHealth}, DamageBonus={damageBonus}, DamageReduction={damageReductionPercent:P0}, MoveSpeed={effectiveMoveSpeed}");
        Debug.Log($"[TestSmoke] WeaponSlot={(equipmentManager.GetSlot(TestEquipmentSlot.Weapon)?.ItemID ?? "None")}");
    }

    [ContextMenu("Test/保存并加载一次")]
    public void SaveAndLoadOnce()
    {
        if (saveLoad == null)
        {
            Debug.LogWarning("TestEquipmentSmokeVerifier: 未绑定 TestGameDataSaveLoad。");
            return;
        }

        saveLoad.SaveGame();
        saveLoad.LoadGame();
        bool ok = true;
        Debug.Log(ok ? "[TestSmoke] Save/Load 成功。" : "[TestSmoke] Save/Load 失败。");
    }
}
