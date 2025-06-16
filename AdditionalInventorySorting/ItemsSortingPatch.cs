using System.Collections.Generic;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;
using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace AdditionalInventorySorting;

[HarmonyPatch]
public class ItemsSortingPatch
{
    public static bool InjectComparers()
    {
        PluginConfig config = Plugin.PluginConfig;

        List<ItemsSorting> worthSort = [];
        if (config.SortByWorthDescEnabled.Value) worthSort.Add(ItemsSortingExtended.ByWorthDesc);
        if (config.SortByWorthAscEnabled.Value) worthSort.Add(ItemsSortingExtended.ByWorthAsc);
        bool byWorth = worthSort.Count == 0 || AddComparerAfter(ItemsSorting.ByPriceDescending, worthSort.ToArray());

        bool byTotalWeight = !config.SortByTotalWeightDescEnabled.Value ||
                             AddComparerAfter(ItemsSorting.ByWeightDescending, ItemsSortingExtended.ByTotalWeightDesc);
        bool byName = !config.SortByNameEnabled.Value ||
                      AddComparerAfter(ItemsSorting.ByNewestDescending, ItemsSortingExtended.AlphabeticalAsc);

#if DEBUG
        void DumpComparers(string name, Il2CppSystem.Collections.Generic.List<ItemsSorting> comparers)
        {
            Plugin.Log.LogInfo($"Comparers in {name}:");
            foreach (ItemsSorting comparer in comparers)
            {
                Plugin.Log.LogInfo($" {comparer.EnumName} - name={comparer._name.Fallback}, locID={comparer._name.ID}");
            }
        }

        DumpComparers(nameof(ItemsSorting.BaseComparers), ItemsSorting.BaseComparers);
        DumpComparers(nameof(ItemsSorting.AllComparers), ItemsSorting.AllComparers);
        DumpComparers(nameof(ItemsSorting.WeaponComparers), ItemsSorting.WeaponComparers);
        DumpComparers(nameof(ItemsSorting.ArmorComparers), ItemsSorting.ArmorComparers);
#endif

        return byWorth && byTotalWeight && byName;
    }

    private static bool AddComparerAfter(ItemsSorting afterValue, params ItemsSorting[] additionalComparers)
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
            case nameof(ItemsSortingExtended.AlphabeticalAsc):
                __result = ExtendedItemComparers.CompareItemName(x, y);
                return false;
        }

        return true;
    }
}
