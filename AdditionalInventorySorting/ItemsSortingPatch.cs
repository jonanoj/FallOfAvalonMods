using System;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;
using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace AdditionalInventorySorting;

[HarmonyPatch]
public class ItemsSortingPatch
{
    public static bool InjectComparers()
    {
        ItemsSorting[] additionalComparers = [ItemsSortingExtended.ByWorthDesc, ItemsSortingExtended.ByWorthAsc];

        bool success = true;
        if (!ListInjection.TryInsertAfter(ItemsSorting.BaseComparers, RichEnumComparer, ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.BaseComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.AllComparers, RichEnumComparer, ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.AllComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.ArmorComparers, RichEnumComparer,
                ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.ArmorComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.WeaponComparers, RichEnumComparer,
                ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.WeaponComparers)}");
        }

#if DEBUG
        foreach (ItemsSorting comparer in ItemsSorting.AllComparers)
        {
            Plugin.Log.LogInfo($"Got comparer: {comparer.EnumName} - name={comparer.Name}, locID={comparer._name.ID}");
        }
#endif

        return success;
    }

    private static bool RichEnumComparer(ItemsSorting x, ItemsSorting y) => x.CompareTo(y) == 0;

    [HarmonyPatch(typeof(ItemsSorting), nameof(ItemsSorting.Compare), typeof(Item), typeof(Item))]
    [HarmonyPrefix]
    public static bool ItemsSortingComparePrefix(ItemsSorting __instance,
        ref int __result,
        [HarmonyArgument(0)] Item x,
        [HarmonyArgument(1)] Item y)
    {
        switch (__instance.EnumName)
        {
            case nameof(ItemsSortingExtended.ByWorthDesc):
                __result = ExtendedItemComparers.ComparePriceToWeightDescending(x, y);
                return false;
            case nameof(ItemsSortingExtended.ByWorthAsc):
                __result = ExtendedItemComparers.ComparePriceToWeightDescending(x, y) * -1;
                return false;
        }

        return true;
    }
}
