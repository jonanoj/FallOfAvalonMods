using BepInEx.Configuration;

namespace CustomDifficulty;

public class PluginConfig
{
    private const string MultiplierDescription = """
                                                 This section contains "multiplier" values for various stats
                                                 1 = means 100% of the original value
                                                 2 = means 200% of the original value
                                                 0.5 = means 50% of the original value
                                                 etc.
                                                 """;

    public const string DifficultyMultipliers = "DifficultyMultipliers";
    public ConfigEntry<float> DamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> DamageReceivedMultiplier { get; private set; }
    public ConfigEntry<float> StaminaUsageMultiplier { get; private set; }
    public ConfigEntry<float> ManaUsageMultiplier { get; private set; }

    private const string DifficultySettings = "DifficultySettings";
    public ConfigEntry<int> AdditionalEnemyAttackingCount { get; private set; }

    private const string ExpMultipliers = "ExpMultipliers";
    public ConfigEntry<float> KillExpMultiplier { get; private set; }
    public ConfigEntry<float> QuestExpMultiplier { get; private set; }
    public ConfigEntry<float> ProficiencyExpMultiplier { get; private set; }

    private const string CoinMultipliers = "CoinMultipliers";
    public ConfigEntry<float> RewardCoinMultiplier { get; private set; }
    public ConfigEntry<float> SellCoinMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            config.Bind(DifficultyMultipliers, "@" + DifficultyMultipliers, "", MultiplierDescription);
            DamageDealtMultiplier = config.Bind(DifficultyMultipliers, nameof(DamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by the player, higher value means more damage is dealt");
            DamageReceivedMultiplier = config.Bind(DifficultyMultipliers, nameof(DamageReceivedMultiplier), 1f,
                "Multiplier for damage dealt to the player, higher value means more damage is received");
            StaminaUsageMultiplier = config.Bind(DifficultyMultipliers, nameof(StaminaUsageMultiplier), 1f,
                "Multiplier for stamina usage, higher value means more stamina is used");
            ManaUsageMultiplier = config.Bind(DifficultyMultipliers, nameof(ManaUsageMultiplier), 1f,
                "Multiplier for mana usage, higher value means more mana is used");

            config.Bind(DifficultySettings, "@" + DifficultySettings, "",
                """
                This section contains "flat" values for various settings
                The value is added as-is to the existing value (rather than multiply it)
                """);
            AdditionalEnemyAttackingCount = config.Bind(DifficultySettings, nameof(AdditionalEnemyAttackingCount), 0,
                "Additional amount of enemies that can attack the player at the same time, this is added to the existing base value (depends on the difficulty setting)");

            config.Bind(ExpMultipliers, "@" + ExpMultipliers, "", MultiplierDescription);
            KillExpMultiplier = config.Bind(ExpMultipliers, nameof(KillExpMultiplier), 1f,
                "Multiplier for EXP gained from enemy kills");
            QuestExpMultiplier = config.Bind(ExpMultipliers, nameof(QuestExpMultiplier), 1f,
                "Multiplier for EXP gained from quests");
            ProficiencyExpMultiplier = config.Bind(ExpMultipliers, nameof(ProficiencyExpMultiplier), 1f,
                """
                Multiplier for proficiency experience gain
                (i.e. One/Two-Handed, Block, Light/Medium/Heavy Armor, Sneak, Cooking, etc.)
                This does NOT affect player level.
                """);

            config.Bind(CoinMultipliers, "@" + CoinMultipliers, "", MultiplierDescription);
            RewardCoinMultiplier = config.Bind(CoinMultipliers, nameof(RewardCoinMultiplier), 1f,
                "Multiplier for coins gained from looting, quests, etc., higher value means more coins are gained");
            SellCoinMultiplier = config.Bind(CoinMultipliers, nameof(SellCoinMultiplier), 1f,
                "Multiplier for coins gained from selling items, higher value means more coins are gained");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
