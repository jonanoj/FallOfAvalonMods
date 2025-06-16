using BepInEx.Configuration;

namespace AdditionalInventorySorting;

public class PluginConfig
{
    public ConfigEntry<bool> SortByNameEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthDescEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthAscEnabled { get; private set; }
    public ConfigEntry<bool> SortByTotalWeightDescEnabled { get; private set; }

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

            ShowWorthInInventory = config.Bind("Inventory", nameof(ShowWorthInInventory), true,
                "Show price/weight ratio next to the item price");

            const string loot = "Loot";
            ShowInfoInLoot = config.Bind(loot, nameof(ShowInfoInLoot), true,
                "Show price and weight in the loot window");
            ShowWorthInLoot = config.Bind(loot, nameof(ShowWorthInLoot), true,
                $"Show price/weight ratio next to the item price in the loot window (will be ignored if {nameof(ShowInfoInLoot)} is set to false)");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
