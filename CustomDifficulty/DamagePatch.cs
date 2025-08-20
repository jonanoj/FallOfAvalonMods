using System;
using System.Collections.Generic;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Fights.DamageInfo;
using Awaken.TG.Main.Fights.NPCs;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Locations.Attachments.Elements;
using BepInEx.Configuration;
using HarmonyLib;

namespace CustomDifficulty;

[HarmonyPatch]
public class DamagePatch
{
    private static readonly Dictionary<DamageOwnerType, ConfigEntry<float>> DamageDealerToMultiplier =
        new()
        {
            { DamageOwnerType.Player, Plugin.PluginConfig.PlayerDamageDealtMultiplier },
            { DamageOwnerType.PlayerSummon, Plugin.PluginConfig.PlayerSummonDamageDealtMultiplier },
            { DamageOwnerType.Enemy, Plugin.PluginConfig.EnemyDamageDealtMultiplier },
            { DamageOwnerType.MiniBoss, Plugin.PluginConfig.MiniBossDamageDealtMultiplier },
            { DamageOwnerType.Boss, Plugin.PluginConfig.BossDamageDealtMultiplier },
        };

    private static readonly Dictionary<DamageOwnerType, ConfigEntry<float>> DamageTargetToMultiplier =
        new()
        {
            { DamageOwnerType.Player, Plugin.PluginConfig.PlayerDamageTakenMultiplier },
            { DamageOwnerType.PlayerSummon, Plugin.PluginConfig.PlayerSummonDamageTakenMultiplier },
            { DamageOwnerType.Enemy, Plugin.PluginConfig.EnemyDamageTakenMultiplier },
            { DamageOwnerType.MiniBoss, Plugin.PluginConfig.MiniBossDamageTakenMultiplier },
            { DamageOwnerType.Boss, Plugin.PluginConfig.BossDamageTakenMultiplier },
        };

    [HarmonyPatch(typeof(Damage), nameof(Damage.Amount), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DamageAmountPostfix(Damage __instance, ref float __result)
    {
        if (!__instance.RawData._finalCalculated)
        {
            return;
        }

        if (__instance.Target is AliveLocation)
        {
            // Don't touch mining/woodcutting/etc
            return;
        }

        float originalValue = __result;

        DamageOwnerType dealerType = DamageOwnerType.Unknown;
        try
        {
            dealerType = OwnerTypeFor(__instance.DamageDealer);
            if (DamageDealerToMultiplier.TryGetValue(dealerType, out ConfigEntry<float> multiplier))
            {
                __result *= multiplier.Value;
            }
            else
            {
                Plugin.Log.LogWarning(
                    $"Damage by unknown source, please report this to the mod author: {__instance.DamageDealer.GetType()}");
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError("Failed to calculate damage dealt, please report this to the mod author\n" + ex);
        }

        DamageOwnerType targetType = DamageOwnerType.Unknown;
        try
        {
            targetType = OwnerTypeFor(__instance.Target);
            if (DamageTargetToMultiplier.TryGetValue(targetType, out ConfigEntry<float> multiplier))
            {
                __result *= multiplier.Value;
            }
            else
            {
                Plugin.Log.LogWarning(
                    $"Damage to unknown target, please report this to the mod author: {__instance.Target.GetType()}");
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError("Failed to calculate damage taken, please report this to the mod author\n" + ex);
        }

        if (Plugin.PluginConfig.DebugLogs.Value)
        {
            Plugin.Log.LogInfo($"{dealerType} -> {targetType} damage changed from {originalValue} to {__result}");
        }
    }

    private static DamageOwnerType OwnerTypeFor(IAlive character)
    {
        return character switch
        {
            Hero => DamageOwnerType.Player,
            NpcElement npc => npc switch
            {
                { IsHeroSummon: true } => DamageOwnerType.PlayerSummon,
                { NpcType: NpcType.Boss } => DamageOwnerType.Boss,
                { NpcType: NpcType.MiniBoss } => DamageOwnerType.MiniBoss,
                _ => DamageOwnerType.Enemy
            },
            _ => DamageOwnerType.Unknown
        };
    }
}
