using Awaken.TG.Main.Character;
using HarmonyLib;

namespace FallDamageControl;

[HarmonyPatch(typeof(FallDamageUtil), nameof(FallDamageUtil.DealFallDamage))]
public class FallDamageUtilPatch
{
    public static void Prefix(ICharacter _, ref float damageToDeal)
    {
        damageToDeal *= Plugin.PluginConfig.FallDamageMultiplier.Value;
    }
}
