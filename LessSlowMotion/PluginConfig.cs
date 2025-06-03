using BepInEx.Configuration;

namespace LessSlowMotion;

public class PluginConfig
{
    public ConfigEntry<int> MinimumKillCameraCooldownSecs { get; private set; }
    public ConfigEntry<int> MinimumSlowMotionCooldownSecs { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            MinimumKillCameraCooldownSecs = config.Bind("LessSlowMotion", "MinimumKillCameraCooldownSecs", 120,
                "Minimum delay in seconds between kill cameras.");
            MinimumSlowMotionCooldownSecs = config.Bind("LessSlowMotion", "MinimumSlowMotionCooldownSecs", 30,
                """
                Minimum delay in seconds between slow motion events.
                This cover non-kill camera slow motions like backstabs or critical hits
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
