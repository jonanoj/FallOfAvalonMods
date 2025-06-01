using Awaken.TG.Main.Character;
using HarmonyLib;

namespace HigherSummonLimit;

[HarmonyPatch]
public class HeroStatsPatch
{
    [HarmonyPatch(typeof(HeroStats), nameof(HeroStats.OnInitialize))]
    [HarmonyPostfix]
    public static void OnInitializePostfix(HeroStats __instance)
    {
        // I originally tried overriding SummonLimit itself, but it doesn't seem to be called.
        // I also tried reverting the summon limit to the original value in a prefix patch to Serialize,
        // but the game crashes if you have more summons than your original limit
        Plugin.Log.LogInfo($"{nameof(HeroStats)}.{nameof(HeroStats.OnInitialize)} called, " +
                           $"original summon limit is {__instance.SummonLimit.BaseValue}, setting to {Plugin.PluginConfig.SummonLimitOverride.Value}");

        __instance.SummonLimit.SetTo(Plugin.PluginConfig.SummonLimitOverride.Value);
    }
}
