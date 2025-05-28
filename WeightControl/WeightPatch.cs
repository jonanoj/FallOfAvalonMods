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
            if (__instance.IsAlchemyComponent
                || __instance.IsConsumable
                || __instance.IsPlainFood
                || __instance.IsPotion
                || __instance.IsCraftingComponent
                || __instance.IsRecipe
                || __instance.IsReadable
                || ItemUtils.IsOther(__instance))
            {
                __result = 0f;
            }
        }
    }
}