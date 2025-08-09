using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace DualTwoHanded;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    internal static PluginConfig PluginConfig;

    public Harmony HarmonyInstance { get; set; }

    public void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loading...");

        PluginConfig = new PluginConfig(Config);
        HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Config.SettingChanged += OnSettingsChanged;

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    private void OnSettingsChanged(object __sender, SettingChangedEventArgs __e)
    {
        ItemEquipPatch.ClearCache();
    }

    public void OnDestroy()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        Config.SettingChanged -= OnSettingsChanged;
        HarmonyInstance?.UnpatchSelf();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
    }
}
