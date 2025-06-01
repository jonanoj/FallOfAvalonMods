using BepInEx.Configuration;

namespace ModName;

public class PluginConfig
{
    // TODO: add config if needed
    // public ConfigEntry<bool> MyValue { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            // TODO: add config bindings as needed
            // MyValue = config.Bind("Category", "ValueName", DefaultValue, "Description");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
