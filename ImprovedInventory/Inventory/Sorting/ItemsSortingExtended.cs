using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;
using ImprovedInventory.Utils;

namespace ImprovedInventory.Inventory.Sorting;

public class ItemsSortingExtended : ItemsSorting
{
    public static readonly ItemsSortingExtended ByWorthDesc = new(nameof(ByWorthDesc),
        Plugin.LanguageConfig.SortByWorthDescDisplayName.Value, ExtendedItemComparers.ComparePriceToWeightDescending);

    public static readonly ItemsSortingExtended ByWorthAsc = new(nameof(ByWorthAsc),
        Plugin.LanguageConfig.SortByWorthAscDisplayName.Value, ExtendedItemComparers.ComparePriceToWeightDescending,
        true);

    public static readonly ItemsSortingExtended ByTotalWeightDesc = new(nameof(ByTotalWeightDesc),
        Plugin.LanguageConfig.SortByTotalWeightDescDisplayName.Value,
        ExtendedItemComparers.CompareTotalWeightDescending);

    public static readonly ItemsSortingExtended AlphabeticalAsc = new(nameof(AlphabeticalAsc),
        Plugin.LanguageConfig.SortByNameDisplayName.Value, ExtendedItemComparers.CompareItemName);

    public static readonly ItemsSortingExtended ByArmorWorthDesc = new(nameof(ByArmorWorthDesc),
        Plugin.LanguageConfig.SortByArmorWorthDescDisplayName.Value,
        ExtendedItemComparers.CompareArmorToWeightDescending);

    private ItemsSortingExtended(string name, string displayName, Comparer comparer, bool reverse = false) :
        base(name, comparer, reverse, "")
    {
        LocStringUtils.Initialize(_name, nameof(ItemsSortingExtended), name, displayName);
    }

    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember<ItemsSorting>(ByWorthDesc) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsSorting>(ByWorthAsc) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsSorting>(ByTotalWeightDesc) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsSorting>(AlphabeticalAsc) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsSorting>(ByArmorWorthDesc);
    }
}
