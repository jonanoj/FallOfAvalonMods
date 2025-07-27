using Awaken.TG.Main.Heroes.Animations;
using HarmonyLib;

namespace HandRegrow;

[HarmonyPatch]
public class HeroOffHandCutOffPatch
{
    [HarmonyPatch(typeof(HeroOffHandCutOff), nameof(HeroOffHandCutOff.ApplyAnimPP))]
    [HarmonyPrefix]
    public static bool ApplyAnimPpPrefix()
    {
        // This patch prevents the hand cut-off visual effect from being applied
        // when loading a save where you already have the effect.
        Plugin.Log.LogInfo($"{nameof(HeroOffHandCutOff)}.{nameof(HeroOffHandCutOff.ApplyAnimPP)} called, " +
                           $"preventing visual effects.");
        return false;
    }

    [HarmonyPatch(typeof(HeroOffHandCutOff), nameof(HeroOffHandCutOff.ApplyAnimPP))]
    [HarmonyPostfix]
    public static void ApplyAnimPpPostfix(HeroOffHandCutOff __instance)
    {
        // Called here because
        //  * Hero.OnFullyInitialized
        //  * HeroOffHandCutOff.OnRestored
        //  * HeroOffHandCutOff.OnInitialized
        // all seemed to be too early, and threw an "Element wasn't fully initialized" error when calling RemoveElementsOfType
        RegrowHandHandler.RegrowHand(__instance.ParentModel);
    }
}
