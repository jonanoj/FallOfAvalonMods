using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DamageNumbers;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    internal static PluginConfig PluginConfig;

    public Harmony HarmonyInstance { get; set; }

    public static Plugin Instance { get; set; }

    public void Awake()
    {
        Instance = this;
        Log = Logger;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loading...");

        PluginConfig = new PluginConfig(Config);

        HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public void OnDestroy()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HeroPatch.Dispose();

        HarmonyInstance?.UnpatchSelf();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
    }
}
