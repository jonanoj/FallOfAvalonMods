using System;
using Awaken.TG.Main.Locations.Containers;
using HarmonyLib;
using Il2CppSystem.Linq;
using UnityEngine.UIElements;

namespace AdditionalInventorySorting;

public class PContainerOneTimePatch
{
    private static readonly Harmony HarmonyInstance = new(nameof(PContainerOneTimePatch));

    public static void Patch()
    {
        HarmonyInstance.PatchAll(typeof(PContainerOneTimePatch));
    }

    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }

    private const float DefaultMaxWidth = 600f;
    private const float PatchedMaxWidth = DefaultMaxWidth * 1.5f;

    [HarmonyPatch(typeof(PContainerUI), nameof(PContainerUI.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void PContainerUIOnFullyInitializedPostfix(PContainerUI __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo("Patching container width");
#endif
        VisualElement rootElement = __instance.Content.Children()
            .FirstOrDefault(new Func<VisualElement, bool>(child => child.name == "root"));
        if (rootElement == null)
        {
            Plugin.Log.LogWarning("Couldn't find item pickup UI element");
            return;
        }

        if (rootElement.m_Style.maxWidth.value >= PatchedMaxWidth)
        {
            // Already patched before or width was changed by the game devs
            return;
        }

        rootElement.style.maxWidth = new StyleLength(PatchedMaxWidth);
        rootElement.style.width = StyleKeyword.Auto;

#if DEBUG
        Plugin.Log.LogInfo("Container width patched, removing patch");
#endif
        HarmonyInstance.UnpatchSelf();
    }
}
