using BepInEx;
using BepInEx.Configuration;
namespace WeightControl;

public class PluginConfig
{
    public ConfigEntry<bool> DisableAlchemyComponents { get; set; }
    public ConfigEntry<bool> DisableConsumables { get; set; }
    public ConfigEntry<bool> DisablePlainFood { get; set; }
    public ConfigEntry<bool> DisablePotions { get; set; }
    public ConfigEntry<bool> DisableCraftingComponents { get; set; }
    public ConfigEntry<bool> DisableRecipes { get; set; }
    public ConfigEntry<bool> DisableReadables { get; set; }
    public ConfigEntry<bool> DisableOther { get; set; }
    public ConfigEntry<bool> DisableUnequipped { get; set; }


    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            DisableAlchemyComponents = config.Bind("Weight", "AlchemyComponents", true, "true = Disable weight for alchemy components, false = keep vanilla weight behavior.");
            DisableConsumables = config.Bind("Weight", "Consumables", true, "tsrue = Disable weight for consumables, false = keep vanilla weight behavior.");
            DisablePlainFood = config.Bind("Weight", "Food", true, "true = Disable weight for food, false = keep vanilla weight behavior.");
            DisablePotions = config.Bind("Weight", "Potions", true, "true = Disable weight for potions, false = keep vanilla weight behavior.");
            DisableCraftingComponents = config.Bind("Weight", "CraftingComponents", true, "true = Disable weight for crafting components, false = keep vanilla weight behavior.");
            DisableRecipes = config.Bind("Weight", "Recipes", true, "true = Disable weight for recipes, false = keep vanilla weight behavior.");
            DisableReadables = config.Bind("Weight", "Readables", true, "true = Disable weight for readables, false = keep vanilla weight behavior.");
            DisableOther = config.Bind("Weight", "Other", true, "true = Disable weight for other items, false = keep vanilla weight behavior.");
            DisableUnequipped = config.Bind("Weight", "Unequipped", false, "true = Disable weight for unequipped items, false = keep vanilla weight behavior.");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}