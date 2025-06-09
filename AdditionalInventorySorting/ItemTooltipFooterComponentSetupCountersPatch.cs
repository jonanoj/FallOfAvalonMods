using System.Globalization;
using Awaken.TG.Main.Heroes.Items.Tooltips.Components;
using Awaken.TG.Main.Heroes.Items.Tooltips.Descriptors;
using HarmonyLib;

namespace AdditionalInventorySorting;

[HarmonyPatch(typeof(ItemTooltipFooterComponent), nameof(ItemTooltipFooterComponent.SetupCounters))]
public class ItemTooltipFooterComponentSetupCountersPatch
{
    public static void Postfix(ItemTooltipFooterComponent __instance,
        [HarmonyArgument(0)] IItemDescriptor itemDescriptor)
    {
        float ratio = ExtendedItemComparers.GetPriceToWeightRatio(itemDescriptor.Price, itemDescriptor.Weight);
        string ratioText = float.IsInfinity(ratio) ? "∞" : ratio.ToString(CultureInfo.InvariantCulture);
        __instance.priceText.text = $"({ratioText}) {itemDescriptor.Price.ToString()}";
    }
}
