using System.Linq;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Animations;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Items.Loadouts;
using Awaken.TG.Main.Scenes.SceneConstructors;
using Awaken.TG.Main.Templates;

namespace HandRegrow;

public static class RegrowHandHandler
{
    public static void RegrowHand(Hero hero)
    {
        if (!hero.HasElement<HeroOffHandCutOff>())
        {
            // Player does not have a hand cut-off effect, nothing to do here.
            return;
        }

        Plugin.Log.LogInfo("Found off-hand cut-off effect, regrowing hand...");
        hero.RemoveElementsOfType<HeroOffHandCutOff>();
        CleanLoadouts(hero);
    }

    private static void CleanLoadouts(Hero hero)
    {
        Item handCutOffItems = hero.HeroItems.OwnedItems.FirstOrDefault(item =>
            item.Template.InheritsFrom(CommonReferences.Get.HandCutOffItemTemplate));

        foreach (HeroLoadout loadout in hero.HeroItems.Loadouts)
        {
            RemoveSlotLock(loadout);
            RemoveHandCutOffItem(loadout, handCutOffItems);
        }
    }

    private static void RemoveHandCutOffItem(HeroLoadout loadout, Item handCutOffItems)
    {
        if (handCutOffItems == null)
        {
            Plugin.Log.LogWarning("No HandCutOffItem found, game code was probably changed");
        }
        else
        {
            loadout.Unequip(handCutOffItems);
        }
    }

    private static void RemoveSlotLock(HeroLoadout loadout)
    {
        var lockers = loadout.Elements<HeroLoadoutSlotLocker>()
            .Where(locker => locker.SlotTypeLocked == EquipmentSlotType.OffHand)
            .ToList();

        if (lockers.Count == 0)
        {
            Plugin.Log.LogWarning("No OffHand HeroLoadoutSlotLocker found, game code was probably changed");
        }
        else
        {
            foreach (HeroLoadoutSlotLocker locker in lockers)
            {
                loadout.RemoveElement(locker);
            }
        }
    }
}
