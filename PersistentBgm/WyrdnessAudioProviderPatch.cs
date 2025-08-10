using Awaken.TG.Main.AudioSystem.Biomes;
using HarmonyLib;

namespace PersistentBgm;

[HarmonyPatch]
public class WyrdnessAudioProviderPatch
{
    [HarmonyPatch(typeof(WyrdnessAudioProvider), nameof(WyrdnessAudioProvider.IsPlayerWithinZone))]
    [HarmonyPrefix]
    public static bool IsPlayerWithinZonePrefix(ref bool __result)
    {
        if (!Plugin.PluginConfig.DisableWyrdnessAmbience.Value)
        {
            return true;
        }

        __result = false;
        return false;
    }
}
