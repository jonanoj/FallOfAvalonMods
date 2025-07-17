using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace WeightControl;

[HarmonyPatch(typeof(Item), nameof(Item.Weight), MethodType.Getter)]
public class ItemWeightPatch
{
    public static bool Prefix(Item __instance, ref float __result)
    {
        if (
            (__instance.IsAlchemyComponent && Plugin.PluginConfig.DisableAlchemyComponents.Value)
            || (__instance.IsConsumable && Plugin.PluginConfig.DisableConsumables.Value)
            || (__instance.IsPlainFood && Plugin.PluginConfig.DisablePlainFood.Value)
            || (__instance.IsPotion && Plugin.PluginConfig.DisablePotions.Value)
            || (__instance.IsCraftingComponent && Plugin.PluginConfig.DisableCraftingComponents.Value)
            || (__instance.IsRecipe && Plugin.PluginConfig.DisableRecipes.Value)
            || (__instance.IsReadable && Plugin.PluginConfig.DisableReadables.Value)
            || (__instance.IsGem && Plugin.PluginConfig.DisableRelics.Value)
            || (__instance.IsEquippable && !__instance.IsEquipped && Plugin.PluginConfig.DisableUnequipped.Value)
            || (__instance.Tool != null && Plugin.PluginConfig.DisableTools.Value)
            || (__instance.IsArrow && Plugin.PluginConfig.DisableArrows.Value)
            || (Plugin.PluginConfig.DisableOther.Value && __instance.IsOther())
        )
        {
            __result = 0f;
            return false;
        }

        return true;
    }
}
