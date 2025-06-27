using System;
using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;
using Awaken.TG.Main.Heroes.Items;
using Il2CppInterop.Runtime;

namespace AdditionalInventorySorting.Inventory.Sorting;

public static class ItemsSortingExtended
{
    public static readonly ItemsSorting ByWorthDesc = New(nameof(ByWorthDesc),
        Plugin.LanguageConfig.SortByWorthDescDisplayName.Value, ExtendedItemComparers.ComparePriceToWeightDescending,
        false);

    public static readonly ItemsSorting ByWorthAsc = New(nameof(ByWorthAsc),
        Plugin.LanguageConfig.SortByWorthAscDisplayName.Value, ExtendedItemComparers.ComparePriceToWeightDescending,
        true);

    public static readonly ItemsSorting ByTotalWeightDesc = New(nameof(ByTotalWeightDesc),
        Plugin.LanguageConfig.SortByTotalWeightDescDisplayName.Value,
        ExtendedItemComparers.CompareTotalWeightDescending, false);

    public static readonly ItemsSorting AlphabeticalAsc = New(nameof(AlphabeticalAsc),
        Plugin.LanguageConfig.SortByNameDisplayName.Value, ExtendedItemComparers.CompareItemName, false);

    private static ItemsSorting New(string name, string displayName, Func<Item, Item, int> comparer, bool reverse)
    {
        return new ItemsSorting(name, ConvertComparer(comparer), reverse, "")
        {
            _name = LocStringUtils.New(nameof(ItemsSortingExtended), name, displayName)
        };
    }

    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember(ByWorthDesc) &&
               RichEnumPatcher.AddOrUpdateMember(ByWorthAsc) &&
               RichEnumPatcher.AddOrUpdateMember(ByTotalWeightDesc) &&
               RichEnumPatcher.AddOrUpdateMember(AlphabeticalAsc);
    }

    private static ItemsSorting.Comparer ConvertComparer(Func<Item, Item, int> comparer)
    {
        return DelegateSupport.ConvertDelegate<ItemsSorting.Comparer>(comparer);
    }
}
