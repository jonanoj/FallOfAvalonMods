using Awaken.TG.Main.Character;
using HarmonyLib;

namespace CustomDifficulty.ExpMultipliers;

[HarmonyPatch]
public class ProficiencyStatsPatch
{
    [HarmonyPatch(typeof(ProficiencyStats), nameof(ProficiencyStats.TryAddXP))]
    [HarmonyPrefix]
    public static void TryAddXpPostfix([HarmonyArgument(1)] ref float amountOfXPToAdd)
    {
        if (amountOfXPToAdd > 0)
        {
#if DEBUG
            float before = amountOfXPToAdd;
#endif
            amountOfXPToAdd *= Plugin.PluginConfig.ProficiencyExpMultiplier.Value;
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(ProficiencyStats)}.{nameof(ProficiencyStats.TryAddXP)} - Changed proficiency XP: {before} -> {amountOfXPToAdd}");
#endif
        }
    }
}
