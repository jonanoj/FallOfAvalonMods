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
        bool success = AddComparerAfter(ItemsSorting.ByPriceDescending,
                           [ItemsSortingExtended.ByWorthDesc, ItemsSortingExtended.ByWorthAsc]) &&
                       AddComparerAfter(ItemsSorting.ByWeightDescending, [ItemsSortingExtended.ByTotalWeightDesc]);

#if DEBUG
        foreach (ItemsSorting comparer in ItemsSorting.AllComparers)
        {
            Plugin.Log.LogInfo($"Got comparer: {comparer.EnumName} - name={comparer.Name}, locID={comparer._name.ID}");
        }
#endif

        return success;
    }

    private static bool AddComparerAfter(ItemsSorting afterValue, ItemsSorting[] additionalComparers)
    {
        bool success = true;
        if (!ListInjection.TryInsertAfter(ItemsSorting.BaseComparers, RichEnumComparer, afterValue,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.BaseComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.AllComparers, RichEnumComparer, afterValue, additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.AllComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.ArmorComparers, RichEnumComparer, afterValue,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.ArmorComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.WeaponComparers, RichEnumComparer, afterValue,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.WeaponComparers)}");
        }

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
            case nameof(ItemsSortingExtended.ByTotalWeightDesc):
                __result = ExtendedItemComparers.CompareTotalWeightDescending(x, y);
                return false;
        }

        return true;
    }
}
