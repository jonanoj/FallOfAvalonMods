using BepInEx.Configuration;

namespace CustomDifficulty;

public class PluginConfig
{
    public const string Multipliers = "DifficultyMultipliers";
    public ConfigEntry<float> DamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> DamageReceivedMultiplier { get; private set; }
    public ConfigEntry<float> StaminaUsageMultiplier { get; private set; }
    public ConfigEntry<float> ManaUsageMultiplier { get; private set; }

    public const string Settings = "DifficultySettings";
    public ConfigEntry<int> AdditionalEnemyAttackingCount { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            config.Bind(Multipliers, "@" + Multipliers, "",
                """
                This section contains "multiplier" values for various stats
                1 = means 100% of the original value
                2 = means 200% of the original value
                0.5 = means 50% of the original value
                etc.
                """);
            DamageDealtMultiplier = config.Bind(Multipliers, nameof(DamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by the player, higher value means more damage is dealt");
            DamageReceivedMultiplier = config.Bind(Multipliers, nameof(DamageReceivedMultiplier), 1f,
                "Multiplier for damage dealt to the player, higher value means more damage is received");
            StaminaUsageMultiplier = config.Bind(Multipliers, nameof(StaminaUsageMultiplier), 1f,
                "Multiplier for stamina usage, higher value means more stamina is used");
            ManaUsageMultiplier = config.Bind(Multipliers, nameof(ManaUsageMultiplier), 1f,
                "Multiplier for mana usage, higher value means more mana is used");

            config.Bind(Settings, "@" + Settings, "",
                """
                This section contains "flat" values for various settings
                The value is added as-is to the existing value (rather than multiply it)
                """);
            AdditionalEnemyAttackingCount = config.Bind(Settings, nameof(AdditionalEnemyAttackingCount), 0,
                "Additional amount of enemies that can attack the player at the same time, this is added to the existing base value (depends on the difficulty setting)");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
