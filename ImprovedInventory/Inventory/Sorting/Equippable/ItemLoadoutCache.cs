using System.Collections.Generic;
using System.Text;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Items.Loadouts;
using Awaken.TG.MVC.Elements;
using HarmonyLib;

namespace ImprovedInventory.Inventory.Sorting.Equippable;

// There's already an ItemUtils.IsUsedInLoadout method, but it is VERY inefficient and causes sorting to take a LOT of time
// So I implemented this basic (and lazy) cache instead to keep sorting as fast as possible
public static class ItemLoadoutCache
{
    private static readonly Harmony HarmonyInstance = new(nameof(ItemLoadoutCache));
    private static HashSet<string> ItemIdsInLoadout = [];
    private static readonly object Lock = new();

    public static void Patch()
    {
        HarmonyInstance.PatchAll(typeof(ItemLoadoutCache));
    }

    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }

    public static bool IsInLoadout(this Item item)
    {
        // Lock isn't checked here because it shouldn't be possible to change your loadouts while sorting items
        // A RW-lock seems a bit too much here, and this function should be as fast as possible
        return ItemIdsInLoadout.Contains(item.ID);
    }

    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void HeroOnFullyInitializedPostfix(Hero __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(Hero)}.{nameof(Hero.OnFullyInitialized)} called - caching loadout items");
#endif
        RecreateCache(__instance.HeroItems.Loadouts);
    }

    [HarmonyPatch(typeof(HeroLoadout), nameof(HeroLoadout.InternalAssignItem))]
    [HarmonyPostfix]
    public static void HeroLoadoutInternalAssignItemPostfix()
    {
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(HeroLoadout)}.{nameof(HeroLoadout.InternalAssignItem)} called - caching loadout items");
#endif
        RecreateCache(Hero.Current.HeroItems.Loadouts);
    }

    private static void RecreateCache(ModelsSet<HeroLoadout> loadouts)
    {
        lock (Lock)
        {
            var newCache = new HashSet<string>();
            foreach (HeroLoadout loadout in loadouts)
            {
                if (loadout?.PrimaryItem != null)
                {
                    newCache.Add(loadout.PrimaryItem.ID);
                }

                if (loadout?.SecondaryItem != null)
                {
                    newCache.Add(loadout.SecondaryItem.ID);
                }
            }

#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(RecreateCache)} - cache ({newCache.Count} items) [{newCache.Join()}]");
#endif

            // Swap cache with a new one (rather than call Clear) for the rare case where someone might actually sort while cache is being recreated
            ItemIdsInLoadout = newCache;
        }
    }
}
