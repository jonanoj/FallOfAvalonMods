using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.CharacterSheet.Overviews.Tabs.CharacterStats;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Items.Attachments;
using Awaken.TG.Main.Heroes.Items.Weapons;
using HarmonyLib;

namespace DualTwoHanded;

[HarmonyPatch]
public class ItemEquipPatch
{
    private static readonly Dictionary<ItemEquip, bool> Cache = [];

    public static void ClearCache()
    {
#if DEBUG
        MethodBase caller = new StackTrace().GetFrame(1).GetMethod();
        Plugin.Log.LogInfo($"Clearing ItemEquip cache on - called by {caller.Name}");
#endif
        Cache.Clear();
    }

    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPrefix]
    public static void HeroOnFullyInitializedPrefix()
    {
        // Clear cache when a save is loaded
        ClearCache();
    }

    [HarmonyPatch(typeof(CharacterStatsUI), nameof(CharacterStatsUI.ApplyStatValues))]
    [HarmonyPostfix]
    public static void CharacterStatsUIApplyStatValuesPostfix()
    {
        // Clear cache when stats are changed
        ClearCache();
    }

    [HarmonyPatch(typeof(ItemEquip), nameof(ItemEquip.EquipmentType), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ItemEquipEquipmentTypePostfix(ItemEquip __instance, ref EquipmentType __result)
    {
        if (__result != EquipmentType.TwoHanded)
        {
            return;
        }

        if (Cache.TryGetValue(__instance, out bool cachedResult))
        {
            if (cachedResult)
            {
                __result = EquipmentType.OneHanded;
            }

            return;
        }

#if DEBUG
        Plugin.Log.LogInfo("Checking item: " + __instance.Item.DisplayName);
#endif

        if (ShouldIgnoreItem(__instance.Item.Template))
        {
            Cache[__instance] = false;
            return;
        }

        if (Plugin.PluginConfig.RequireStat.Value && !CheckStats(__instance))
        {
            Cache[__instance] = false;
            return;
        }

        Cache[__instance] = true;
        __result = EquipmentType.OneHanded;
    }

    private static bool CheckStats(ItemEquip __instance)
    {
        ItemStatsRequirements requirements = __instance.Item.StatsRequirements;
        return requirements == null || HasEnoughStats(Hero.Current.HeroRPGStats, requirements);
    }

    private static bool HasEnoughStats(HeroRPGStats stats, ItemStatsRequirements req)
    {
        if (req == null)
        {
            return true;
        }

        float reqStrength = req.StrengthRequired * Plugin.PluginConfig.RequiredStrengthMultiplier.Value;
        return stats.Strength > reqStrength;
    }

    private static bool ShouldIgnoreItem(ItemTemplate item)
    {
        // Keep pickaxes, shovels, fishing rods, etc. as two-handed tools
        return !item.IsMelee || item.IsTool;
    }
}
