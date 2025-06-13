using BepInEx.Configuration;

namespace CustomKeybinds;

public class PluginConfig
{
    public ConfigEntry<bool> IncludeDebugKeys { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            IncludeDebugKeys = config.Bind("Keybinds", "IncludeDebugKeys", false,
                """
                Set to true to allow configuring the cheat keys (a.k.a Debug keys)
                Example debug keys:
                  U - Give the player Gold, EXP, Ethereal Webs, etc.
                  ` (Backquote) - Toggle god-mode
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
