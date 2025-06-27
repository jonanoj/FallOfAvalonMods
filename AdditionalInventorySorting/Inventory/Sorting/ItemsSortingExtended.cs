using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;

namespace AdditionalInventorySorting.Inventory.Sorting;

public static class ItemsSortingExtended
{
    public static readonly ItemsSorting ByWorthDesc = New(nameof(ByWorthDesc),
        Plugin.LanguageConfig.SortByWorthDescDisplayName.Value, false);

    public static readonly ItemsSorting ByWorthAsc = New(nameof(ByWorthAsc),
        Plugin.LanguageConfig.SortByWorthAscDisplayName.Value, true);

    public static readonly ItemsSorting ByTotalWeightDesc = New(nameof(ByTotalWeightDesc),
        Plugin.LanguageConfig.SortByTotalWeightDescDisplayName.Value, false);

    public static readonly ItemsSorting AlphabeticalAsc = New(nameof(AlphabeticalAsc),
        Plugin.LanguageConfig.SortByNameDisplayName.Value, false);

    private static ItemsSorting New(string name, string displayName, bool reverse)
    {
        return new ItemsSorting(name, null /* Implementation is injected through ItemsSortingPatch */, reverse, "")
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
}
