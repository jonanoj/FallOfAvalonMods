using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ModName;

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

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public void OnDestroy()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HarmonyInstance?.UnpatchSelf();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
    }
}
