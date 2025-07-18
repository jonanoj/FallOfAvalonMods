using BepInEx.Configuration;

namespace HigherSummonLimit;

public class PluginConfig
{
    public ConfigEntry<int> AdditionalSummonLimit { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            AdditionalSummonLimit = config.Bind("Summons", "AdditionalSummonLimit", 7,
                "Increase the maximum number of summons allowed");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
