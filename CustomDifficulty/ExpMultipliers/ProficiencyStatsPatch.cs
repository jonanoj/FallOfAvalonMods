using Awaken.TG.Main.Character;
using Awaken.TG.Main.General.StatTypes;
using BepInEx.Configuration;
using HarmonyLib;

namespace CustomDifficulty.ExpMultipliers;

[HarmonyPatch]
public class ProficiencyStatsPatch
{
    [HarmonyPatch(typeof(ProficiencyStats), nameof(ProficiencyStats.TryAddXP))]
    [HarmonyPrefix]
    public static void TryAddXpPostfix([HarmonyArgument(0)] ProfStatType statType,
        [HarmonyArgument(1)] ref float amountOfXPToAdd)
    {
        if (amountOfXPToAdd > 0)
        {
#if DEBUG
            float before = amountOfXPToAdd;
#endif
            amountOfXPToAdd *= Plugin.PluginConfig.ProficiencyExpMultiplier.Value;
            if (Plugin.PluginConfig.ProfExpMultipliers.TryGetValue(statType.EnumName, out ConfigEntry<float> profMultiplier))
            {
                amountOfXPToAdd *= profMultiplier.Value;
            }
            else
            {
                Plugin.Log.LogError($"No multiplier found for ${statType.EnumName} - this should never happen");
            }
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(ProficiencyStats)}.{nameof(ProficiencyStats.TryAddXP)} - Changed proficiency XP: {before} -> {amountOfXPToAdd}");
#endif
        }
    }
}
