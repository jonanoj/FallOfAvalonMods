using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HigherWeightLimit;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    internal static PluginConfig PluginConfig;

    public Harmony HarmonyInstance { get; set; }

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loading...");

        PluginConfig = new PluginConfig(Config);
        HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public override bool Unload()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HarmonyInstance?.UnpatchSelf();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
        return true;
    }
}
