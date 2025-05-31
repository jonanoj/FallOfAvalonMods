using BepInEx.Configuration;

namespace FallDamageControl;

public class PluginConfig
{
    public ConfigEntry<float> FallDamageMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            FallDamageMultiplier = config.Bind("FallDamage", "FallDamageMultiplier", 0.5f,
                """
                Fall damage multiplier.

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
