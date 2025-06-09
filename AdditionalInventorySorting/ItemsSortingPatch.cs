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
        Func<ItemsSorting, ItemsSorting, bool> richEnumComparer = (x, y) => x.CompareTo(y) == 0;
        if (!ListInjection.TryInsertAfter(ItemsSorting.BaseComparers, richEnumComparer, ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.BaseComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.AllComparers, richEnumComparer, ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.AllComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.ArmorComparers, richEnumComparer,
                ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.ArmorComparers)}");
        }

        if (!ListInjection.TryInsertAfter(ItemsSorting.WeaponComparers, richEnumComparer,
                ItemsSorting.ByPriceDescending,
                additionalComparers))
        {
            success = false;
            Plugin.Log.LogWarning(
                $"Failed to add custom {nameof(ItemsSorting)} to {nameof(ItemsSorting.WeaponComparers)}");
        }

#if DEBUG
        foreach (var comparer in ItemsSorting.AllComparers)
        {
            Plugin.Log.LogInfo($"Got comparer: {comparer.EnumName} - name={comparer.Name}, locID={comparer._name.ID}");
        }
#endif

        return success;
    }

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
