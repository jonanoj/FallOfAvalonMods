using System.Reflection;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;

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

        ItemsTabType.Readable.SetSubTabs([
            ItemsTabTypeExtended.ReadableUnread,
            ItemsTabTypeExtended.ReadableRead
        ]);
    }

    public static void AddRecipesSubTabs()
    {
        if (ItemsTabType.Recipes.SubTabs != null)
        {
            Plugin.Log.LogWarning("Recipes sub-tabs already exist, not adding new tabs");
            return;
        }

        ItemsTabType.Recipes.SetSubTabs([
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

        ItemsTabType.Potion.SetSubTabs([
            ItemsTabTypeExtended.PotionHealth,
            ItemsTabTypeExtended.PotionMana,
            ItemsTabTypeExtended.PotionStamina
        ]);
    }

    private static void SetSubTabs(this ItemsTabType itemsTabType, ItemsTabType[] subTabs)
    {
        var field = typeof(ItemsTabType).GetField($"<{nameof(ItemsTabType.SubTabs)}>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(itemsTabType, subTabs);
        }
        else
        {
            Plugin.Log.LogError("Failed to find backing field for SubTabs.");
        }
    }
}
