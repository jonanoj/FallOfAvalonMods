using BepInEx.Configuration;

namespace ImprovedStorage;

public class LanguageConfig
{
    public ConfigEntry<string> TransferStackDisplayName { get; private set; }

    public ConfigEntry<string> TransferAllDisplayName { get; private set; }

    public LanguageConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string language = "Language";
            TransferStackDisplayName = config.Bind(language, nameof(TransferStackDisplayName), "Transfer Stack",
                "Display name of the 'Transfer Stack' prompt, you can modify this if you're playing in a different language");
            TransferAllDisplayName = config.Bind(language, nameof(TransferAllDisplayName), "Transfer All",
                "Display name of the 'Transfer All' prompt, you can modify this if you're playing in a different language");
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
