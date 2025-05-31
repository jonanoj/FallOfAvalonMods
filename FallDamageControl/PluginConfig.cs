using BepInEx.Configuration;

namespace FallDamageControl;

public class PluginConfig
{
    public ConfigEntry<FallDamageNegationMode> NegationMode { get; private set; }
    public ConfigEntry<float> FallDamageMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            NegationMode = config.Bind("FallDamage", "NegationMode",
                FallDamageNegationMode.NonLethal,
                """
                NonLethal = fall damage will always leave you with at least 1 HP.
                MaxHealth = fall damage will always leave you with at least 1 HP only when you're at max health.
                Vanilla = vanilla fall damage behavior.
                """);
            FallDamageMultiplier = config.Bind("FallDamage", "FallDamageMultiplier", 1f,
                """
                Fall damage multiplier
                For example:
                  0 = no fall damage
                  0.5 = 50% fall damage
                  1.0 = 100% fall damage
                  2.0 = 200% fall damage
                  etc.
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
