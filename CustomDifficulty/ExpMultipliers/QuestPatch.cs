using Awaken.TG.Main.Stories.Quests;
using HarmonyLib;

namespace CustomDifficulty.ExpMultipliers;

[HarmonyPatch]
public class QuestPatch
{
    [HarmonyPatch(typeof(Quest), nameof(Quest.ExperiencePoints), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ExperiencePointsPostfix(ref float __result)
    {
#if DEBUG
        float before = __result;
#endif
        __result *= Plugin.PluginConfig.QuestExpMultiplier.Value;
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(Quest)}.{nameof(Quest.ExperiencePoints)} - Changed quest EXP: {before} -> {__result}");
#endif
    }
}
