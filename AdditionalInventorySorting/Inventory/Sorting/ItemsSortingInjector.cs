using System.Collections.Generic;
using AdditionalInventorySorting.Config;
using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;

namespace AdditionalInventorySorting.Inventory.Sorting;

public static class ItemsSortingInjector
{
    public static bool InjectComparers()
    {
        PluginConfig config = Plugin.PluginConfig;

        List<ItemsSorting> worthSort = [];
        if (config.SortByWorthDescEnabled.Value) worthSort.Add(ItemsSortingExtended.ByWorthDesc);
        if (config.SortByWorthAscEnabled.Value) worthSort.Add(ItemsSortingExtended.ByWorthAsc);
        bool byWorth = worthSort.Count == 0 || AddComparerAfter(ItemsSorting.ByPriceDescending, [.. worthSort]);

        bool byTotalWeight = !config.SortByTotalWeightDescEnabled.Value ||
                             AddComparerAfter(ItemsSorting.ByWeightDescending, ItemsSortingExtended.ByTotalWeightDesc);
        bool byName = !config.SortByNameEnabled.Value ||
                      AddComparerAfter(ItemsSorting.ByNewestDescending, ItemsSortingExtended.AlphabeticalAsc);

#if DEBUG
        static void DumpComparers(string name, Il2CppSystem.Collections.Generic.List<ItemsSorting> comparers)
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
}
