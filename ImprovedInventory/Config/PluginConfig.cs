using ImprovedInventory.Inventory.Sorting.Equippable;
using BepInEx.Configuration;

namespace ImprovedInventory.Config;

public class PluginConfig
{
    public ConfigEntry<bool> SortByNameEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthDescEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthAscEnabled { get; private set; }
    public ConfigEntry<bool> SortByTotalWeightDescEnabled { get; private set; }
    public ConfigEntry<bool> SortByArmorWorthDescEnabled { get; private set; }

    public ConfigEntry<EquipSortModes> EquipSortMode { get; private set; }

    public ConfigEntry<bool> InventoryTabsReadableEnabled { get; private set; }
    public ConfigEntry<bool> InventoryTabsRecipesEnabled { get; private set; }
    public ConfigEntry<bool> InventoryTabsPotionEnabled { get; private set; }

    public ConfigEntry<bool> ShowWorthInInventory { get; private set; }

    public ConfigEntry<bool> ShowInfoInLoot { get; private set; }
    public ConfigEntry<bool> ShowWorthInLoot { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string itemSorting = "ItemSorting";
            SortByNameEnabled = config.Bind(itemSorting, nameof(SortByNameEnabled), true,
                "Add sorting by item name (alphabetical order)");
            SortByWorthDescEnabled = config.Bind(itemSorting, nameof(SortByWorthDescEnabled), true,
                "Add sorting by price/weight ratio descending (bigger values first)");
            SortByWorthAscEnabled = config.Bind(itemSorting, nameof(SortByWorthAscEnabled), true,
                "Add sorting by price/weight ratio ascending (smaller values first)");
            SortByTotalWeightDescEnabled = config.Bind(itemSorting, nameof(SortByTotalWeightDescEnabled), true,
                "Add sorting by total weight of items in slot/stack");
            SortByArmorWorthDescEnabled = config.Bind(itemSorting, nameof(SortByArmorWorthDescEnabled), true,
                "Add sorting by armor/weight ratio descending (bigger values first)");

            EquipSortMode = config.Bind(itemSorting, nameof(EquipSortMode), EquipSortModes.EquippedFirst,
                $"""
                 Allows sorting equipped items separately from unequipped items.
                 This setting applies to *all* item sort types - including both base game and those added by this mod (Newest, Price, Weight, Price/Weight, etc.)
                 Possible values:
                   {nameof(EquipSortModes.EquippedFirst)} - Equipped items appear before everything else.
                   {nameof(EquipSortModes.EquippedLast)}  - Equipped items appear after everything else.
                   {nameof(EquipSortModes.Vanilla)}       - Same as the base game, equipped items aren't treated any differently.
                 """);

            const string inventoryTabs = "InventoryTabs";
            InventoryTabsReadableEnabled = config.Bind(inventoryTabs, nameof(InventoryTabsReadableEnabled), true,
                """
                Add Unread/Read sub-tabs to the readable items tab in your inventory.
                Note that sub-tabs aren't updated when you read an item, so you will have to switch tabs to see the change.
                """);
            InventoryTabsRecipesEnabled = config.Bind(inventoryTabs, nameof(InventoryTabsRecipesEnabled), true,
                """
                Add Unread/Read sub-tabs to the recipes tab in your inventory.
                Note that sub-tabs aren't updated when you read an item, so you will have to switch tabs to see the change.
                """);
            InventoryTabsPotionEnabled = config.Bind(inventoryTabs, nameof(InventoryTabsPotionEnabled), true,
                """
                Add Potion sub-tabs to the potions tab in your inventory.
                Added tabs are:
                 - Health
                 - Mana
                 - Stamina
                 - Other
                """);

            const string inventory = "Inventory";
            ShowWorthInInventory = config.Bind(inventory, nameof(ShowWorthInInventory), true,
                "Show price/weight ratio next to the item price");

            const string loot = "Loot";
            ShowInfoInLoot = config.Bind(loot, nameof(ShowInfoInLoot), true,
                "Show price and weight in the loot window");
            ShowWorthInLoot = config.Bind(loot, nameof(ShowWorthInLoot), false,
                $"Show price/weight ratio next to the item price in the loot window (will be ignored if {nameof(ShowInfoInLoot)} is set to false)");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
