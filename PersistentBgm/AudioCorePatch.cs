using Awaken.TG.Main.AudioSystem.Biomes;
using HarmonyLib;
using UnityEngine;

namespace PersistentBgm;

[HarmonyPatch]
public class AudioCorePatch
{
#if DEBUG
    private static float LastCombatLevel;
#endif

    [HarmonyPatch(typeof(AudioCore), nameof(AudioCore.DetermineMusicToPlay))]
    public static bool Prefix([HarmonyArgument(0)] ref float combatLevel)
    {
        if (Plugin.PluginConfig.DisableBattleMusic.Value && combatLevel > 0f)
        {
            combatLevel = 0f;
        }

#if DEBUG
        if (!Mathf.Approximately(combatLevel, LastCombatLevel))
        {
            LastCombatLevel = combatLevel;
            Plugin.Log.LogInfo(
                $"{nameof(AudioCore)}.{nameof(AudioCore.DetermineMusicToPlay)} called with new combat level: {combatLevel}");
        }
#endif

        return true;
    }

#if DEBUG
    [HarmonyPatch(typeof(VSPBiomeManager), nameof(VSPBiomeManager.UpdateBiomeSounds))]
    [HarmonyPrefix]
    public static bool UpdateBiomeSoundsPrefix(VSPBiomeManager __instance)
    {
        Plugin.Log.LogWarning(
            $"{nameof(VSPBiomeManager)}.{nameof(VSPBiomeManager.UpdateBiomeSounds)} called - current biome: {__instance.BiomeType}");
        return true;
    }

    [HarmonyPatch(typeof(AudioCore), nameof(AudioCore.PlayCombatMusic))]
    [HarmonyPrefix]
    public static bool PlayCombatMusicPrefix()
    {
        Plugin.Log.LogWarning($"{nameof(AudioCore)}.{nameof(AudioCore.PlayCombatMusic)} called");
        return true;
    }

    [HarmonyPatch(typeof(AudioCore), nameof(AudioCore.PlayAlertMusic))]
    [HarmonyPrefix]
    public static bool PlayAlertMusicPrefix()
    {
        Plugin.Log.LogWarning($"{nameof(AudioCore)}.{nameof(AudioCore.PlayAlertMusic)} called");
        return true;
    }
#endif
}
