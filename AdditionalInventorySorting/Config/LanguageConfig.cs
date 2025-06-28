using BepInEx.Configuration;

namespace AdditionalInventorySorting.Config;

public class LanguageConfig
{
    public ConfigEntry<string> SortByNameDisplayName { get; private set; }
    public ConfigEntry<string> SortByWorthDescDisplayName { get; private set; }
    public ConfigEntry<string> SortByWorthAscDisplayName { get; private set; }
    public ConfigEntry<string> SortByTotalWeightDescDisplayName { get; private set; }
    public ConfigEntry<string> SortByArmorWorthDescDisplayName { get; private set; }

    public ConfigEntry<string> ReadableSubTypeUnreadDisplayName { get; private set; }
    public ConfigEntry<string> ReadableSubTypeReadDisplayName { get; private set; }
    public ConfigEntry<string> PotionSubTypeHealthDisplayName { get; private set; }
    public ConfigEntry<string> PotionSubTypeManaDisplayName { get; private set; }
    public ConfigEntry<string> PotionSubTypeStaminaDisplayName { get; private set; }


    public LanguageConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string language = "Language";
            SortByNameDisplayName = config.Bind(language, nameof(SortByNameDisplayName), "Name",
                "Display name to show for Name sort type, you can modify this if you're playing in a different language");
            SortByWorthDescDisplayName = config.Bind(language, nameof(SortByWorthDescDisplayName), "Price/Weight",
                "Display name to show for Price/Weight sort type, you can modify this if you're playing in a different language");
            SortByWorthAscDisplayName = config.Bind(language, nameof(SortByWorthAscDisplayName),
                "Price/Weight (Ascending)",
                "Display name to show for Price/Weight (Ascending) sort type, you can modify this if you're playing in a different language");
            SortByTotalWeightDescDisplayName = config.Bind(language, nameof(SortByTotalWeightDescDisplayName),
                "Total Weight",
                "Display name to show for Total Weight sort type, you can modify this if you're playing in a different language");
            SortByArmorWorthDescDisplayName = config.Bind(language, nameof(SortByArmorWorthDescDisplayName),
                "Armor/Weight",
                "Display name to show for Armor/Weight sort type, you can modify this if you're playing in a different language");

            ReadableSubTypeUnreadDisplayName = config.Bind(language, nameof(ReadableSubTypeUnreadDisplayName),
                "Unread",
                "Display name to show for Readable sub-tab Unread, you can modify this if you're playing in a different language");
            ReadableSubTypeReadDisplayName = config.Bind(language, nameof(ReadableSubTypeReadDisplayName),
                "Read",
                "Display name to show for Readable sub-tab Read, you can modify this if you're playing in a different language");

            PotionSubTypeHealthDisplayName = config.Bind(language, nameof(PotionSubTypeHealthDisplayName),
                "Health",
                "Display name to show for Potion sub-tab Health, you can modify this if you're playing in a different language");
            PotionSubTypeManaDisplayName = config.Bind(language, nameof(PotionSubTypeManaDisplayName),
                "Mana",
                "Display name to show for Potion sub-tab Mana, you can modify this if you're playing in a different language");
            PotionSubTypeStaminaDisplayName = config.Bind(language, nameof(PotionSubTypeStaminaDisplayName),
                "Stamina",
                "Display name to show for Potion sub-tab Stamina, you can modify this if you're playing in a different language");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
