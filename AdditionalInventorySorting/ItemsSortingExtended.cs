using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;

namespace AdditionalInventorySorting;

public static class ItemsSortingExtended
{
    public static readonly ItemsSorting ByWorthDesc = new(
        nameof(ByWorthDesc),
        null, // This will be handled by the patch
        false,
        "") { _name = LocStringUtils.New(nameof(ItemsSortingExtended), nameof(ByWorthDesc), "Price/Weight") };

    public static readonly ItemsSorting ByWorthAsc = new(
        nameof(ByWorthAsc),
        null, // This will be handled by the patch
        true,
        "")
    {
        _name = LocStringUtils.New(nameof(ItemsSortingExtended), nameof(ByWorthAsc), "Price/Weight (Ascending)")
    };

    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember(nameof(ByWorthDesc), ByWorthDesc) &&
               RichEnumPatcher.AddOrUpdateMember(nameof(ByWorthAsc), ByWorthAsc);
    }
}
