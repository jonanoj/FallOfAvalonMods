using BepInEx.Configuration;

namespace LessHoldTime;

public class PluginConfig
{
    public ConfigEntry<float> CraftHoldTimeMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            CraftHoldTimeMultiplier = config.Bind("HoldTime", nameof(CraftHoldTimeMultiplier), 0.1f,
                """
                Percent to reduce the craft key hold time by
                For example:
                    0.1 = 10%
                    0.2 = 20%
                    0.5 = 50%
                    1.0 = 100% (vanilla)
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
