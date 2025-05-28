using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace WeightControl;

public class WeightPatch
{
    [HarmonyPatch(typeof(Item), nameof(Item.Weight), MethodType.Getter)]
    public class ItemWeightPatch
    {
        public static void Postfix(ref float __result)
        {
            __result = 0f;
        }
    }
}