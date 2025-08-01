using Awaken.TG.Main.Fights.NPCs;
using HarmonyLib;

namespace CustomDifficulty.ExpMultipliers;

[HarmonyPatch]
public class NpcTemplatePatch
{
    [HarmonyPatch(typeof(NpcTemplate), nameof(NpcTemplate.GetExpReward))]
    [HarmonyPostfix]
    public static void GetExpRewardPostfix(NpcTemplate __instance, ref int __result)
    {
#if DEBUG
        int before = __result;
#endif
        if (__result > 0)
        {
            __result = (int)(__result * Plugin.PluginConfig.KillExpMultiplier.Value);
        }
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(NpcTemplate)}.{nameof(NpcTemplate.GetExpReward)} - Changed kill EXP: {before} -> {__result}");
#endif
    }
}
