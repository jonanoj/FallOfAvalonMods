using Awaken.TG.Main.Settings.Gameplay;
using HarmonyLib;

namespace CustomDifficulty;

[HarmonyPatch]
public class DifficultyPatch
{
    [HarmonyPatch(typeof(Difficulty), nameof(Difficulty.DamageDealt), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DamageDealtPostfix(Difficulty __instance, ref float __result)
    {
        __result *= Plugin.PluginConfig.DamageDealtMultiplier.Value;
    }

    [HarmonyPatch(typeof(Difficulty), nameof(Difficulty.DamageReceived), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DamageReceivedPostfix(Difficulty __instance, ref float __result)
    {
        __result *= Plugin.PluginConfig.DamageReceivedMultiplier.Value;
    }

    [HarmonyPatch(typeof(Difficulty), nameof(Difficulty.ManaUsage), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ManaUsagePostfix(Difficulty __instance, ref float __result)
    {
        __result *= Plugin.PluginConfig.ManaUsageMultiplier.Value;
    }

    [HarmonyPatch(typeof(Difficulty), nameof(Difficulty.StaminaUsage), MethodType.Getter)]
    [HarmonyPostfix]
    public static void StaminaUsagePostfix(Difficulty __instance, ref float __result)
    {
        __result *= Plugin.PluginConfig.StaminaUsageMultiplier.Value;
    }

    [HarmonyPatch(typeof(Difficulty), nameof(Difficulty.MaxEnemiesAttacking), MethodType.Getter)]
    [HarmonyPostfix]
    public static void MaxEnemiesAttackingPostfix(Difficulty __instance, ref int __result)
    {
        __result += Plugin.PluginConfig.AdditionalEnemyAttackingCount.Value;
    }
}
