using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Stats;
using Awaken.TG.Main.Heroes.Stats.Tweaks;
using Awaken.TG.MVC;
using HarmonyLib;

namespace HigherWeightLimit;

[HarmonyPatch]
public class HeroRpgStatsPatch
{
    [HarmonyPatch(typeof(HeroRPGStats), nameof(HeroRPGStats.AfterHeroFullyInitialized))]
    [HarmonyPostfix]
    public static void HeroRpgStatsAfterHeroFullyInitializedPostfix()
    {
        Hero hero = Hero.Current;
        if (hero == null)
        {
            Plugin.Log.LogError("Player object doesn't exist, can't add weight");
            return;
        }

        TweakSystem tweakSystem = World.Services.Get<TweakSystem>();
        if (tweakSystem == null)
        {
            Plugin.Log.LogError("TweakSystem is null, can't add weight");
            return;
        }

        float weightMultiplier = Plugin.PluginConfig.WeightMultiplier.Value;

        Stat encumbranceLimit = hero.HeroStats.EncumbranceLimit;
        float originalWeight = encumbranceLimit.ModifiedValue;

        tweakSystem.AddTweak(tweakSystem.Tweak(
            encumbranceLimit,
            StatTweak.Multi(encumbranceLimit, weightMultiplier, TweakPriority.Multiply),
            TweakPriority.Multiply));

        Stat armorWeightMultiplier = hero.HeroStats.ArmorWeightMultiplier;
        float originalArmorWeightMultiplier = armorWeightMultiplier.ModifiedValue;

        tweakSystem.AddTweak(tweakSystem.Tweak(
            armorWeightMultiplier,
            StatTweak.Multi(armorWeightMultiplier, weightMultiplier, TweakPriority.Multiply),
            TweakPriority.Multiply));

        ArmorWeight armorWeight = hero.TryGetElement<ArmorWeight>();
        Plugin.Log.LogInfo($"Weight patched:\n" +
                           $"  Weight limit changed from {originalWeight} to {encumbranceLimit.ModifiedValue}\n" +
                           $"  Armor weight multiplier changed from {originalArmorWeightMultiplier} to {armorWeightMultiplier.ModifiedValue}\n" +
                           $"  Max armor weight is {armorWeight?.MaxEquipmentWeight}");
    }
}
