using System;
using System.Globalization;
using Awaken.TG.Main.Heroes.Items;

namespace AdditionalInventorySorting;

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

    public static int ComparePriceToWeightDescending(Item item1, Item item2)
    {
        bool zeroWeight1 = item1.Weight == 0f;
        bool zeroWeight2 = item2.Weight == 0f;

        // Show items with weight first
        if (!zeroWeight1 && zeroWeight2)
            return -1;
        if (zeroWeight1 && !zeroWeight2)
            return 1;

        // Both weightless - sort by price descending
        if (zeroWeight1 && zeroWeight2)
            return item2.Price.CompareTo(item1.Price);

        // Both have weight: sort by price-to-weight ratio descending
        float x = GetPriceToWeightRatio(item1.Price, item1.Weight);
        float y = GetPriceToWeightRatio(item2.Price, item2.Weight);
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
