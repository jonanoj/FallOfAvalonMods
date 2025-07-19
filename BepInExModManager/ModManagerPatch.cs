using System.Collections.Generic;
using Awaken.TG.Main.UI.Menu.ModManager;
using Awaken.TG.Main.UI.Popup;
using Awaken.TG.MVC;
using Awaken.Utility.Assets.Modding;
using HarmonyLib;

namespace BepInExModManager;

[HarmonyPatch]
public class ModManagerPatch
{
    [HarmonyPatch(typeof(ModEntryUI), nameof(ModEntryUI.ToggleActive))]
    [HarmonyPrefix]
    public static bool ModEntryUIToggleActivePrefix(ModEntryUI __instance)
    {
        if (__instance.Index < 0)
        {
            PopupUI.SpawnNoChoicePopup(typeof(VSmallPopupUI),
                """
                BepInEx mods cannot be safely toggled on/off through the in-game mod manager.
                Please remove the mod manually from the BepInEx plugins folder and restart your game.
                """,
                "Error");
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(ModManagerUI), nameof(ModManagerUI.InitializeModEntries))]
    [HarmonyPostfix]
    public static void InitializeModEntries(ModManagerUI __instance)
    {
        List<ModMetadata> mods = ModDetails.GetAllLoadedMods();
        for (int i = 0; i < mods.Count; i++)
        {
            ModMetadata bepInExPlugin = mods[i];
            // Negative index is used to quickly identify added mods in the other patch functions
            var modEntry = new BepInExModEntryUI(-1 - i, bepInExPlugin);
            __instance.AddElement(modEntry);
            World.SpawnView<VModEntryUI>(modEntry, true, true, __instance.View.EntriesParent);
        }
    }

    [HarmonyPatch(typeof(ModManagerUI), nameof(ModManagerUI.ChangeModPosition))]
    [HarmonyPrefix]
    public static bool ChangeModPosition(ModManagerUI __instance)
    {
        if (__instance._selectedModEntryUI != null && __instance._selectedModEntryUI.Index < 0)
        {
            PopupUI.SpawnNoChoicePopup(typeof(VSmallPopupUI),
                "BepInEx mod load order can't be controlled through the in-game mod manager.",
                "Error");
            return false;
        }

        return true;
    }
}
