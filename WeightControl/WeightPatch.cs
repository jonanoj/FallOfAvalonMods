using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace WeightControl;

public class WeightPatch
{
    [HarmonyPatch(typeof(Item), nameof(Item.Weight), MethodType.Getter)]
    public class ItemWeightPatch
    {
        public static void Postfix(Item __instance, ref float __result)
        {
            if (
                (__instance.IsAlchemyComponent && Plugin.PluginConfig.DisableAlchemyComponents.Value)
                || (__instance.IsConsumable && Plugin.PluginConfig.DisableConsumables.Value)
                || (__instance.IsPlainFood && Plugin.PluginConfig.DisablePlainFood.Value)
                || (__instance.IsPotion && Plugin.PluginConfig.DisablePotions.Value)
                || (__instance.IsCraftingComponent && Plugin.PluginConfig.DisableCraftingComponents.Value)
                || (__instance.IsRecipe && Plugin.PluginConfig.DisableRecipes.Value)
                || (__instance.IsReadable && Plugin.PluginConfig.DisableReadables.Value)
                || (__instance.IsEquippable && !__instance.IsEquipped && Plugin.PluginConfig.DisableUnequipped.Value)
                || (Plugin.PluginConfig.DisableOther.Value && ItemUtils.IsOther(__instance))
            )
            {
                __result = 0f;
            }
        }
    }
}