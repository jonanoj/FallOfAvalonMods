using System;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.MVC;
using HarmonyLib;

namespace FallDamageControl;

[HarmonyPatch(typeof(FallDamageUtil), nameof(FallDamageUtil.DealFallDamage))]
public class FallDamageUtilPatch
{
    public static void Prefix(ICharacter character, ref float damageToDeal)
    {
        float originalFallDamage = damageToDeal;

        damageToDeal *= Plugin.PluginConfig.FallDamageMultiplier.Value;

        if (Plugin.PluginConfig.NegationMode.Value == FallDamageNegationMode.Vanilla)
        {
            return;
        }

        var hero = World.Any<Hero>();
        if (hero == null)
        {
            Plugin.Log.LogWarning("Player not found, skipping fall damage adjustment.");
            return;
        }

        damageToDeal = Plugin.PluginConfig.NegationMode.Value switch
        {
            FallDamageNegationMode.NonLethal => (int)Math.Min(damageToDeal, hero.AliveStats.Health - 1),
            FallDamageNegationMode.MaxHealth => (int)Math.Min(damageToDeal, hero.AliveStats.MaxHealth - 1),
            _ => damageToDeal
        };

        Plugin.Log.LogDebug($"Dealing fall damage: {originalFallDamage} -> {damageToDeal} " +
                            $"(Multiplier: {Plugin.PluginConfig.FallDamageMultiplier.Value}, NegationMode: {Plugin.PluginConfig.NegationMode.Value})");
    }
}
