using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Locations.Containers;
using Awaken.TG.Main.UIToolkit;
using HarmonyLib;
using ImprovedInventory.Inventory.Sorting;

namespace ImprovedInventory.Loot;

public class PContainerElementPatch
{
    private static readonly Harmony HarmonyInstance = new(nameof(PContainerElementPatch));

    public static void Patch()
    {
        HarmonyInstance.PatchAll(typeof(PContainerElementPatch));
    }

    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }

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


        string pricePerWeight = $"{item.Price}$/{item.Weight}kg";
        return Plugin.PluginConfig.ShowWorthInLoot.Value
            ? $"({pricePerWeight} = {ExtendedItemComparers.GetValueToRatioString(item.Price, item.Weight, 0)})" // Round to 0 digits to add as little width as possible
            : $"({pricePerWeight})";
    }
}
