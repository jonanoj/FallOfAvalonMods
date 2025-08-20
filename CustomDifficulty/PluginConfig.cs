using System.Collections.Generic;
using System.Linq;
using Awaken.TG.Main.General.StatTypes;
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

    private const string DamageDealtMultipliers = "DamageDealtMultipliers";
    public ConfigEntry<float> PlayerDamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> PlayerSummonDamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> EnemyDamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> MiniBossDamageDealtMultiplier { get; private set; }
    public ConfigEntry<float> BossDamageDealtMultiplier { get; private set; }

    private const string DamageTakenMultipliers = "DamageTakenMultipliers";
    public ConfigEntry<float> PlayerDamageTakenMultiplier { get; private set; }
    public ConfigEntry<float> PlayerSummonDamageTakenMultiplier { get; private set; }
    public ConfigEntry<float> EnemyDamageTakenMultiplier { get; private set; }
    public ConfigEntry<float> MiniBossDamageTakenMultiplier { get; private set; }
    public ConfigEntry<float> BossDamageTakenMultiplier { get; private set; }

    public const string DifficultyMultipliers = "DifficultyMultipliers";
    public ConfigEntry<float> StaminaUsageMultiplier { get; private set; }
    public ConfigEntry<float> ManaUsageMultiplier { get; private set; }

    private const string DifficultySettings = "DifficultySettings";
    public ConfigEntry<int> AdditionalEnemyAttackingCount { get; private set; }

    private const string ExpMultipliers = "ExpMultipliers";
    public ConfigEntry<float> KillExpMultiplier { get; private set; }
    public ConfigEntry<float> QuestExpMultiplier { get; private set; }
    public ConfigEntry<float> ProficiencyExpMultiplier { get; private set; }


    private const string ProficiencyExpMultipliers = "ProficiencyExpMultipliers";
    public Dictionary<string, ConfigEntry<float>> ProfExpMultipliers { get; } = [];


    private const string CoinMultipliers = "CoinMultipliers";
    public ConfigEntry<float> RewardCoinMultiplier { get; private set; }
    public ConfigEntry<float> SellCoinMultiplier { get; private set; }

    public ConfigEntry<bool> DebugLogs { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            config.Bind(DamageDealtMultipliers, "@" + DamageDealtMultipliers, "", MultiplierDescription);
            PlayerDamageDealtMultiplier = config.Bind(DamageDealtMultipliers, nameof(PlayerDamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by the player to all targets");
            PlayerSummonDamageDealtMultiplier = config.Bind(DamageDealtMultipliers,
                nameof(PlayerSummonDamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by player summons to all targets");
            EnemyDamageDealtMultiplier = config.Bind(DamageDealtMultipliers, nameof(EnemyDamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by normal enemies to all targets");
            MiniBossDamageDealtMultiplier = config.Bind(DamageDealtMultipliers, nameof(MiniBossDamageDealtMultiplier),
                1f, "Multiplier for damage dealt by mini bosses to all targets");
            BossDamageDealtMultiplier = config.Bind(DamageDealtMultipliers, nameof(BossDamageDealtMultiplier), 1f,
                "Multiplier for damage dealt by bosses to all targets");

            config.Bind(DamageTakenMultipliers, "@" + DamageTakenMultipliers, "", MultiplierDescription);
            PlayerDamageTakenMultiplier = config.Bind(DamageTakenMultipliers, nameof(PlayerDamageTakenMultiplier), 1f,
                "Multiplier for damage taken by the player from all sources");
            PlayerSummonDamageTakenMultiplier = config.Bind(DamageTakenMultipliers,
                nameof(PlayerSummonDamageTakenMultiplier), 1f,
                "Multiplier for damage taken by player summons from all sources");
            EnemyDamageTakenMultiplier = config.Bind(DamageTakenMultipliers, nameof(EnemyDamageTakenMultiplier), 1f,
                "Multiplier for damage taken by normal enemies from all sources");
            MiniBossDamageTakenMultiplier = config.Bind(DamageTakenMultipliers, nameof(MiniBossDamageTakenMultiplier),
                1f, "Multiplier for damage taken by mini bosses from all sources");
            BossDamageTakenMultiplier = config.Bind(DamageTakenMultipliers, nameof(BossDamageTakenMultiplier), 1f,
                "Multiplier for damage taken by bosses from all sources");


            config.Bind(DifficultyMultipliers, "@" + DifficultyMultipliers, "", MultiplierDescription);
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

            config.Bind(ProficiencyExpMultipliers, "@" + ProficiencyExpMultipliers, "", MultiplierDescription);
            foreach (string proficiency in ProfStatType.HeroProficiencies.Select(p => p.EnumName))
            {
                ConfigEntry<float> entry = config.Bind(ProficiencyExpMultipliers, proficiency, 1f,
                    $"Multiplier for {proficiency} proficiency experience gain");
                ProfExpMultipliers[proficiency] = entry;
            }

            config.Bind(CoinMultipliers, "@" + CoinMultipliers, "", MultiplierDescription);
            RewardCoinMultiplier = config.Bind(CoinMultipliers, nameof(RewardCoinMultiplier), 1f,
                "Multiplier for coins gained from looting, quests, etc., higher value means more coins are gained");
            SellCoinMultiplier = config.Bind(CoinMultipliers, nameof(SellCoinMultiplier), 1f,
                "Multiplier for coins gained from selling items, higher value means more coins are gained");

            DebugLogs = config.Bind("Debug", nameof(DebugLogs), false,
                "When enabled, debug logs will be written to the BepInEx console");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
