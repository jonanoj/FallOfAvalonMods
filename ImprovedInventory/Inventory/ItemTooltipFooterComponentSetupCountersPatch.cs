using ImprovedInventory.Inventory.Sorting;
using Awaken.TG.Main.Heroes.Items.Tooltips.Components;
using Awaken.TG.Main.Heroes.Items.Tooltips.Descriptors;
using HarmonyLib;

namespace ImprovedInventory.Inventory;

public class ItemTooltipFooterComponentSetupCountersPatch
{
    private static readonly Harmony HarmonyInstance = new(nameof(ItemTooltipFooterComponentSetupCountersPatch));

    public static void Patch()
    {
        HarmonyInstance.PatchAll(typeof(ItemTooltipFooterComponentSetupCountersPatch));
    }

    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }

    [HarmonyPatch(typeof(ItemTooltipFooterComponent), nameof(ItemTooltipFooterComponent.SetupCounters))]
    [HarmonyPostfix]
    public static void ItemTooltipFooterComponentSetupCountersPostfix(ItemTooltipFooterComponent __instance,
        [HarmonyArgument(0)] IItemDescriptor itemDescriptor)
    {
        string ratioText =
            ExtendedItemComparers.GetValueToRatioString(itemDescriptor.Price, itemDescriptor.Weight);
        __instance.priceText.text = $"({ratioText}) {itemDescriptor.Price}";
    }
}
