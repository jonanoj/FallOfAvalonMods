using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace AdditionalInventorySorting;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public Harmony HarmonyInstance { get; set; }

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loading...");

        if (!ItemsSortingExtended.InjectMembers())
        {
            Log.LogError("Failed to inject ItemsSorting RichEnum extensions");
            return;
        }

        if (!ItemsSortingPatch.InjectComparers())
        {
            Log.LogError("Failed to inject custom comparers to ItemSorting list");
            return;
        }

        HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        PContainerOneTimePatch.Patch();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public override bool Unload()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HarmonyInstance?.UnpatchSelf();
        PContainerOneTimePatch.Unpatch();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
        return true;
    }
}
