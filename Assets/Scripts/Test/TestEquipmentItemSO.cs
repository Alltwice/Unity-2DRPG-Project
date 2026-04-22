using UnityEngine;

[CreateAssetMenu(fileName = "NewTestEquipment", menuName = "Test/物品数据/装备", order = 201)]
public class TestEquipmentItemSO : ItemDataSO
{
    [Header("装备属性")]
    [SerializeField] private TestEquipmentSlot slot = TestEquipmentSlot.Weapon;
    [Range(0f, 0.95f)]
    [SerializeField] private float damageReductionPercent;
    [SerializeField] private int damageBonus;
    [SerializeField] private float moveSpeedBonus;
    public override void EquipMethod(GameObject target, int index)
    {
        #region agent log
        TestDebugSessionLogger.Log("pre-fix", "H2", "TestEquipmentItemSO.EquipMethod", "equip method enter", $"slotField={slot}, itemName={ItemName}, itemId={ItemID}, index={index}");
        #endregion
        if (target == null || InventoryManager.Instance == null)
            return;

        if (index < 0 || index >= InventoryManager.Instance.slots.Count)
            return;
        //接受目标需要装备的对象位置和数据
        InventorySlot fromSlot = InventoryManager.Instance.slots[index];
        if (fromSlot == null || fromSlot.instance == null)
            return;

        TestEquipmentManager equipmentManager = target.GetComponent<TestEquipmentManager>();
        if (equipmentManager == null)
            equipmentManager = target.AddComponent<TestEquipmentManager>();

        TestPlayerStatModifiers modifiers = target.GetComponent<TestPlayerStatModifiers>();
        if (modifiers == null)
            modifiers = target.AddComponent<TestPlayerStatModifiers>();
        //拿到该物品的实例化对象
        ItemInstance incoming = fromSlot.instance;
        //装备的同时拿到旧的
        ItemInstance replaced = equipmentManager.Equip(slot, incoming);
        InventoryManager.Instance.RemoveItem(index, 1);

        if (replaced != null && replaced.definition is TestEquipmentItemSO replacedEquipment)
            replacedEquipment.RemoveBonuses(modifiers);
        //增益属性的同时
        ApplyBonuses(modifiers);

        if (replaced != null && replaced.definition != null)
        {
            //把旧的放回去
            InventoryManager.Instance.AddItem(1, replaced.definition);
        }

        GameEvent.TriggerInventoryChanged();
    }

    public void ApplyBonuses(TestPlayerStatModifiers modifiers)
    {
        if (modifiers == null)
            return;

        modifiers.AddModifier(damageReductionPercent, damageBonus, moveSpeedBonus);
    }

    public void RemoveBonuses(TestPlayerStatModifiers modifiers)
    {
        if (modifiers == null)
            return;

        modifiers.RemoveModifier(damageReductionPercent, damageBonus, moveSpeedBonus);
    }
}
