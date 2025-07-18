using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Stats;
using Awaken.TG.Main.Heroes.Stats.Tweaks;
using Awaken.TG.MVC;
using HarmonyLib;

namespace HigherSummonLimit;

[HarmonyPatch]
public class HeroStatsPatch
{
    [HarmonyPatch(typeof(HeroRPGStats), nameof(HeroRPGStats.AfterHeroFullyInitialized))]
    [HarmonyPostfix]
    public static void HeroRpgStatsAfterHeroFullyInitializedPostfix()
    {
        Hero hero = Hero.Current;
        if (hero == null)
        {
            Plugin.Log.LogError("Player object doesn't exist, can't increase summon limit");
            return;
        }

        TweakSystem tweakSystem = World.Services.Get<TweakSystem>();
        if (tweakSystem == null)
        {
            Plugin.Log.LogError("TweakSystem is null, can't increase summon limit");
            return;
        }

        float additionalSummonLimit = Plugin.PluginConfig.AdditionalSummonLimit.Value;

        Stat summonLimit = hero.HeroStats.SummonLimit;
        float originSummonLimit = summonLimit.ModifiedValue;

        tweakSystem.AddTweak(tweakSystem.Tweak(
            summonLimit,
            StatTweak.Add(summonLimit, additionalSummonLimit, TweakPriority.Add),
            TweakPriority.Add));

        Plugin.Log.LogInfo($"Summon limit changed from {originSummonLimit} to {summonLimit.ModifiedValue}");
    }
}
