/// <summary>
/// 背包排序字段
/// </summary>
public enum InventorySortField
{
    Price,
    Rarity
}

/// <summary>
/// 背包排序方向
/// 注意：在当前业务语义中，
/// - Price 的 Ascending 代表“高->低”
/// - Rarity 的 Ascending 代表“好->坏”
/// 具体映射逻辑在 InventoryQueryServiceTagBoost 内处理。
/// </summary>
public enum InventorySortDirection
{
    Ascending,
    Descending
}

