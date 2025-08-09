using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Items.Attachments;
using Awaken.TG.Main.Heroes.Items.Weapons;
using HarmonyLib;

namespace DualTwoHanded;

[HarmonyPatch]
public class ItemEquipPatch
{
    [HarmonyPatch(typeof(ItemEquip), nameof(ItemEquip.EquipmentType), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ItemEquipEquipmentTypePostfix(ItemEquip __instance, ref EquipmentType __result)
    {
        if (__result != EquipmentType.TwoHanded)
        {
            return;
        }

        if (ShouldIgnoreItem(__instance.Item.Template))
        {
            return;
        }

        if (Plugin.PluginConfig.RequireStat.Value && !CheckStats(__instance))
        {
            return;
        }

        __result = EquipmentType.OneHanded;
    }

    private static bool CheckStats(ItemEquip __instance)
    {
        var requirements = __instance.Item.StatsRequirements;
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
