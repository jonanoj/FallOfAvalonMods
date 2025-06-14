using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Locations.Containers;
using Awaken.TG.Main.UIToolkit;
using HarmonyLib;

namespace AdditionalInventorySorting;

[HarmonyPatch]
public class PContainerElementPatch
{
    [HarmonyPatch(typeof(PContainerElement), nameof(PContainerElement.SetData))]
    [HarmonyPostfix]
    public static void PContainerElementSetDataPostfix(PContainerElement __instance, [HarmonyArgument(1)] Item item)
    {
        string priceLabel = ShortPriceLabel(item);
        if (!__instance._quantityLabel.IsActive())
        {
            __instance._quantityLabel.text = priceLabel;
            __instance._quantityLabel.SetActiveOptimized(true);
        }
        else
        {
            __instance._quantityLabel.text += " " + priceLabel;
        }
    }

    private static string ShortPriceLabel(Item item)
    {
        // No point to display weight ratio of 0 weight items
        if (item.Weight == 0)
        {
            return $"({item.Price}$)";
        }

        // Round to 0 digits to save as much width as possible
        return
            $"({item.Price}$/{item.Weight}kg = {ExtendedItemComparers.GetPriceToWeightRatioString(item.Price, item.Weight, 0)})";
    }
}
