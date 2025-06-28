using System;
using System.Globalization;
using Awaken.TG.Main.Heroes.Items;

namespace AdditionalInventorySorting.Inventory.Sorting;

public static class ExtendedItemComparers
{
    public static float GetPriceToWeightRatio(float price, float weight, int roundDigits = 2)
    {
        weight = MathF.Round(weight, 2);

        if (price == 0f && weight == 0f)
            return 0f;

        if (price == 0f)
            return float.PositiveInfinity;

        return MathF.Round(price / weight, roundDigits);
    }

    public static string GetPriceToWeightRatioString(float price, float weight, int roundDigits = 2)
    {
        float ratio = GetPriceToWeightRatio(price, weight, roundDigits);
        string ratioText = float.IsInfinity(ratio) ? "∞" : ratio.ToString(CultureInfo.InvariantCulture);
        return ratioText;
    }

    public static int ComparePriceToWeightDescending(Item item1, Item item2) =>
        CompareItemRatioDescending(item1, item2, ItemPrice);

    private static float ItemPrice(Item item) => item.Price;

    public static int CompareArmorToWeightDescending(Item item1, Item item2) =>
        CompareItemRatioDescending(item1, item2, ItemArmor);

    private static float ItemArmor(Item item) => item.Stat(ItemStatType.ItemArmor)?.ModifiedValue ?? 0f;

    private static int CompareItemRatioDescending(Item item1, Item item2, Func<Item, float> valueGetter)
    {
        float item1Weight = item1.Weight;
        float item2Weight = item2.Weight;
        bool zeroWeight1 = item1Weight == 0f;
        bool zeroWeight2 = item2Weight == 0f;

        // Show items with weight first
        if (!zeroWeight1 && zeroWeight2)
            return -1;
        if (zeroWeight1 && !zeroWeight2)
            return 1;

        float item1Value = valueGetter(item1);
        float item2Value = valueGetter(item2);

        // Both weightless - fallback to raw value descending
        if (zeroWeight1 && zeroWeight2)
            return item2Value.CompareTo(item1Value);

        // Both have weight: sort by ratio descending
        float x = GetPriceToWeightRatio(item1Value, item1Weight);
        float y = GetPriceToWeightRatio(item2Value, item2Weight);
        return y.CompareTo(x);
    }

    public static int CompareTotalWeightDescending(Item item1, Item item2)
    {
        float x = item1.Weight * item1.Quantity;
        float y = item2.Weight * item2.Quantity;
        return y.CompareTo(x);
    }

    public static int CompareItemName(Item item1, Item item2)
    {
        return string.Compare(item1.DisplayName, item2.DisplayName, StringComparison.CurrentCulture);
    }
}
