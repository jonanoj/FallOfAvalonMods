using Awaken.TG.Main.Settings;
using Awaken.TG.Main.Settings.Gameplay;
using Awaken.TG.MVC;
using Awaken.TG.MVC.Events;
using BepInEx.Configuration;

namespace CustomDifficulty;

public static class SettingsEvent
{
    public static void OnSettingChanged(object _, SettingChangedEventArgs args)
    {
        ConfigEntryBase setting = args.ChangedSetting;
#if DEBUG
        Plugin.Log.LogInfo(
            $"Setting changed: {setting.Definition.Key} in section {setting.Definition.Section}");
#endif
        if (setting.Definition.Section != PluginConfig.Multipliers)
        {
            // Only multipliers are listening to the SettingRefresh event
            return;
        }

        EventSystem eventSystem = World.EventSystem;
        if (eventSystem == null)
        {
            Plugin.Log.LogWarning("EventSystem is null, cannot refresh settings");
            return;
        }

        var difficultySetting = World.Only<DifficultySetting>();
        if (difficultySetting._options == null)
        {
            Plugin.Log.LogDebug("Difficulty isn't loaded yet");
            return;
        }

        eventSystem.Trigger(difficultySetting, Setting.Events.SettingRefresh, difficultySetting);
    }
}
