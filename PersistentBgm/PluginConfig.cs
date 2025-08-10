using BepInEx.Configuration;

namespace PersistentBgm;

public class PluginConfig
{
    public ConfigEntry<bool> DisableBattleMusic { get; private set; }
    public ConfigEntry<bool> DisableWyrdnessAmbience { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string section = "BGM";
            DisableBattleMusic = config.Bind(section, nameof(DisableBattleMusic), true,
                "Disables BGM switch when entering combat");
            DisableWyrdnessAmbience = config.Bind(section, nameof(DisableWyrdnessAmbience), true,
                "Disables the ambient BGM when staying outside of safe zones during the wyrd night");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
