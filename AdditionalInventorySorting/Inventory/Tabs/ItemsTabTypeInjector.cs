using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace AdditionalInventorySorting.Inventory.Tabs;

public static class ItemsTabTypeInjector
{
    public static void AddReadableSubTabs()
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

    public static void AddPotionSubTabs()
    {
        if (ItemsTabType.Potion.SubTabs != null)
        {
            Plugin.Log.LogWarning("Potion sub-tabs already exist, not adding new tabs");
            return;
        }

        ItemsTabType.Potion._SubTabs_k__BackingField = new Il2CppReferenceArray<ItemsTabType>([
            ItemsTabTypeExtended.PotionHealth,
            ItemsTabTypeExtended.PotionMana,
            ItemsTabTypeExtended.PotionStamina
        ]);
    }
}
