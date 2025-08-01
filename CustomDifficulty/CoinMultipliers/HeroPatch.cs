using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Stats;
using Awaken.TG.MVC.Events;
using HarmonyLib;

namespace CustomDifficulty.CoinMultipliers;

[HarmonyPatch]
public class HeroPatch
{
    [HarmonyPatch(typeof(Hero), nameof(Hero.ChangingStatWealth))]
    [HarmonyPostfix]
    public static void ChangingStatWealthPostfix(
        [HarmonyArgument(0)] HookResult<IWithStats, Stat.StatChange> statChange)
    {
        if (statChange.Value.value > 0)
        {
#if DEBUG
            float before = statChange.Value.value;
#endif
            var multiplier = statChange.Value.context?.reason == ChangeReason.Trade
                ? Plugin.PluginConfig.SellCoinMultiplier
                : Plugin.PluginConfig.RewardCoinMultiplier;
            statChange.Value = new Stat.StatChange(statChange.Value.stat, statChange.Value.value * multiplier.Value,
                statChange.Value.context);
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(Hero)}.{nameof(Hero.ChangingStatWealth)} - Changed wealth stat: {before} -> {statChange.Value.value}, reason: {statChange.Value.context?.reason.ToString()}");
#endif
        }
    }
}
