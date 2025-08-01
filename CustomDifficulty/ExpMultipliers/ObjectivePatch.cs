using Awaken.TG.Main.Stories.Quests.Objectives;
using HarmonyLib;

namespace CustomDifficulty.ExpMultipliers;

[HarmonyPatch]
public class ObjectivePatch
{
    [HarmonyPatch(typeof(Objective), nameof(Objective.ExperiencePoints), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ExperiencePointsPostfix(ref float __result)
    {
#if DEBUG
        float before = __result;
#endif
        __result *= Plugin.PluginConfig.QuestExpMultiplier.Value;
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(Objective)}.{nameof(Objective.ExperiencePoints)} - Changed objective EXP: {before} -> {__result}");
#endif
    }
}
