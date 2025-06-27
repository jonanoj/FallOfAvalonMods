using AdditionalInventorySorting.Inventory.Sorting.Equippable;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List;
using Awaken.TG.Main.Heroes.Items;
using HarmonyLib;

namespace AdditionalInventorySorting.Inventory.Sorting;

[HarmonyPatch]
public class ItemsSortingPatch
{
    [HarmonyPatch(typeof(ItemsSorting), nameof(ItemsSorting.Compare), typeof(Item), typeof(Item))]
    [HarmonyPrefix]
    public static bool ItemsSortingComparePrefix(ItemsSorting __instance,
        ref int __result,
        [HarmonyArgument(0)] Item x,
        [HarmonyArgument(1)] Item y)
    {
        EquipSortModes equipSortMode = Plugin.PluginConfig.EquipSortMode.Value;
        if (equipSortMode != EquipSortModes.Vanilla)
        {
            int xPriority = GetItemPriority(x);
            int yPriority = GetItemPriority(y);

            if (xPriority != yPriority)
            {
                __result = equipSortMode == EquipSortModes.EquippedFirst
                    ? xPriority.CompareTo(yPriority)
                    : yPriority.CompareTo(xPriority);
                return false;
            }
        }

        return true;
    }

    private static int GetItemPriority(Item item)
    {
        if (item.IsEquipped)
            return 0;
        if (item.IsInLoadout())
            return 1;
        return 2;
    }
}
