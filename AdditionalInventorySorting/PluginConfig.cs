using BepInEx.Configuration;

namespace AdditionalInventorySorting;

public class PluginConfig
{
    public ConfigEntry<bool> SortByNameEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthDescEnabled { get; private set; }
    public ConfigEntry<bool> SortByWorthAscEnabled { get; private set; }
    public ConfigEntry<bool> SortByTotalWeightDescEnabled { get; private set; }

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
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
