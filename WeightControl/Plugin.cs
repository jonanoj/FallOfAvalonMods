using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace WeightControl;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }
}
