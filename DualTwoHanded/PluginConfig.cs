using BepInEx.Configuration;

namespace DualTwoHanded;

public class PluginConfig
{
    public ConfigEntry<bool> RequireStat { get; private set; }
    public ConfigEntry<float> RequiredStrengthMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            RequireStat = config.Bind("TwoHanded", nameof(RequireStat), true,
                """
                When set to false, all two-handed weapons can be equipped in one hand.
                When set to true, the player must have more than the required stat to equip a two-handed weapon in one hand.
                """);
            RequiredStrengthMultiplier = config.Bind("TwoHanded", nameof(RequiredStrengthMultiplier), 2f,
                """
                Multiplies the required strength of a weapon for it to be equipable in one hand.
                By default, a two-handed weapon requires 2x the strength of a one-handed weapon.

                For example:
                  0.5 = 50% of required strength
                  1 = 100% of required strength
                  1.5 = 150% of required strength
                  2 = 200% of required strength (default)
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
