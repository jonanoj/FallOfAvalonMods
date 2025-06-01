using BepInEx.Configuration;

namespace HigherSummonLimit;

public class PluginConfig
{
    public ConfigEntry<int> SummonLimitOverride { get; set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            SummonLimitOverride = config.Bind("Summons", "SummonLimitOverride", 10,
                "Override the maximum number of summons allowed. Set to 3 to revert to the vanilla limit. (or 4/5 if you leveled up the summoning talent)");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
