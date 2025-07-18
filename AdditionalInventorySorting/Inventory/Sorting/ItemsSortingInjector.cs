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
        bool byArmor = !config.SortByArmorWorthDescEnabled.Value ||
                       AddComparerAfter(ItemsSorting.ArmorComparers, nameof(ItemsSorting.ArmorComparers),
                           ItemsSorting.ByArmorDescending, ItemsSortingExtended.ByArmorWorthDesc);

#if DEBUG
        static void DumpComparers(string name, List<ItemsSorting> comparers)
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

        return byWorth && byTotalWeight && byName && byArmor;
    }

    private static bool AddComparerAfter(ItemsSorting afterValue, params ItemsSorting[] additionalComparers)
    {
        return AddComparerAfter(ItemsSorting.BaseComparers, nameof(ItemsSorting.BaseComparers), afterValue,
                   additionalComparers) &&
               AddComparerAfter(ItemsSorting.AllComparers, nameof(ItemsSorting.AllComparers), afterValue,
                   additionalComparers) &&
               AddComparerAfter(ItemsSorting.ArmorComparers, nameof(ItemsSorting.ArmorComparers), afterValue,
                   additionalComparers) &&
               AddComparerAfter(ItemsSorting.WeaponComparers, nameof(ItemsSorting.WeaponComparers), afterValue,
                   additionalComparers);
    }

    private static bool AddComparerAfter(List<ItemsSorting> comparersList,
        string comparerListName, ItemsSorting afterValue, params ItemsSorting[] additionalComparers)
    {
        if (ListInjection.TryInsertAfter(comparersList, RichEnumComparer, afterValue, additionalComparers))
        {
            return true;
        }

        Plugin.Log.LogWarning(
            $"Failed to add custom {nameof(ItemsSorting)} to {comparerListName}");
        return false;
    }

    private static bool RichEnumComparer(ItemsSorting x, ItemsSorting y) => x.CompareTo(y) == 0;
}
