using UnityEngine;

public static class RarityPriceCalculator
{
    public static int ToFinalPrice(int basePrice, ItemRarity rarity)
    {
        if (basePrice <= 0)
            return 0;

        float multiplier = rarity switch
        {
            ItemRarity.普通 => 1.0f,
            ItemRarity.稀有 => 1.5f,
            ItemRarity.史诗 => 2.5f,
            ItemRarity.传奇 => 4.0f,
            _ => 1.0f
        };

        return Mathf.RoundToInt(basePrice * multiplier);
    }
}
