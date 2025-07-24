using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ImprovedInventory.Config;
using ImprovedInventory.Inventory;
using ImprovedInventory.Inventory.Sorting;
using ImprovedInventory.Inventory.Sorting.Equippable;
using ImprovedInventory.Inventory.Tabs;
using ImprovedInventory.Loot;

namespace ImprovedInventory;

[BepInPlugin(PluginConsts.PLUGIN_GUID, PluginConsts.PLUGIN_NAME, PluginConsts.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    internal static PluginConfig PluginConfig;
    internal static LanguageConfig LanguageConfig;
    public Harmony HarmonyInstance { get; set; }

    public void Awake()
    {
        Log = Logger;
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

        if (!ItemsSortingInjector.InjectComparers())
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
            PContainerOneTimePatch.Patch();
        }

        if (PluginConfig.EquipSortMode.Value != EquipSortModes.Vanilla)
        {
            ItemLoadoutCache.Patch();
            ItemsSortingPatch.Patch();
        }

        if (!ItemsTabTypeExtended.InjectMembers())
        {
            Log.LogError("Failed to inject ItemsTabType RichEnum extensions");
        }

        if (PluginConfig.InventoryTabsReadableEnabled.Value)
        {
            ItemsTabTypeInjector.AddReadableSubTabs();
        }

        if (PluginConfig.InventoryTabsRecipesEnabled.Value)
        {
            ItemsTabTypeInjector.AddRecipesSubTabs();
        }

        if (PluginConfig.InventoryTabsPotionEnabled.Value)
        {
            ItemsTabTypeInjector.AddPotionSubTabs();
        }

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is loaded!");
    }

    public void OnDestroy()
    {
        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloading...");

        HarmonyInstance?.UnpatchSelf();
        ItemTooltipFooterComponentSetupCountersPatch.Unpatch();
        PContainerElementPatch.Unpatch();
        PContainerOneTimePatch.Unpatch();
        ItemLoadoutCache.Unpatch();
        ItemsSortingPatch.Unpatch();

        Log.LogInfo($"Plugin {PluginConsts.PLUGIN_GUID} is unloaded!");
    }
}
