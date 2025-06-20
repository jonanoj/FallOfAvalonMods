using BepInEx.Configuration;

namespace HigherWeightLimit;

public class PluginConfig
{
    public ConfigEntry<int> WeightMultiplier { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            WeightMultiplier = config.Bind("Weight", nameof(WeightMultiplier), 25,
                "Multiplies your weight by the given value");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
