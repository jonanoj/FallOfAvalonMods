using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace AdditionalInventorySorting;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    internal static PluginConfig PluginConfig;
    internal static LanguageConfig LanguageConfig;

    public Harmony HarmonyInstance { get; set; }

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loading...");

        PluginConfig = new PluginConfig(Config);
        LanguageConfig = new LanguageConfig(new ConfigFile(
            Utility.CombinePaths(Paths.ConfigPath, PluginConsts.PLUGIN_GUID + ".Language.cfg"), false,
            MetadataHelper.GetMetadata(this)));

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
        if (PluginConfig.ShowWorthInInventory.Value)
        {
            ItemTooltipFooterComponentSetupCountersPatch.Patch();
        }

        if (PluginConfig.ShowInfoInLoot.Value)
        {
            PContainerElementPatch.Patch();
        }

        if (PluginConfig.EquipSortMode.Value != EquipSortModes.Vanilla)
        {
            ItemLoadoutCache.Patch();
        }

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public override bool Unload()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HarmonyInstance?.UnpatchSelf();
        ItemTooltipFooterComponentSetupCountersPatch.Unpatch();
        PContainerElementPatch.Unpatch();
        ItemLoadoutCache.Unpatch();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
        return true;
    }
}
