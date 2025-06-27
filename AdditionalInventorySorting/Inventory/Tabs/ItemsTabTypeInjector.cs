using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace AdditionalInventorySorting.Inventory.Tabs;

public static class ItemsTabTypeInjector
{
    public static void AddReadUnreadTabs()
    {
        if (ItemsTabType.Readable.SubTabs != null)
        {
            Plugin.Log.LogWarning("Readable sub-tabs already exist, not adding new tabs");
            return;
        }

        ItemsTabType.Readable._SubTabs_k__BackingField = new Il2CppReferenceArray<ItemsTabType>([
            ItemsTabTypeExtended.ReadableUnread,
            ItemsTabTypeExtended.ReadableRead
        ]);
    }
}
