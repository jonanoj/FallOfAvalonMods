using Awaken.TG.Main.Locations.Containers;
using HarmonyLib;
using UnityEngine.UIElements;

namespace ImprovedInventory.Loot;

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

    private const float DefaultMinWidth = 400f; // private const float DefaultMaxWidth = 600f;
    private const float PatchedMinWidth = DefaultMinWidth;
    private const float PatchedMaxWidth = 900f;

    [HarmonyPatch(typeof(PContainerUI), nameof(PContainerUI.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void PContainerUIOnFullyInitializedPostfix(PContainerUI __instance)
    {
        try
        {
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(PContainerUI)}.{nameof(PContainerUI.OnFullyInitialized)} - extending container width");
#endif
            VisualElement rootElement = __instance.Content.Q<VisualElement>("root");
            if (rootElement == null)
            {
                Plugin.Log.LogWarning("Couldn't find item pickup UI element");
                return;
            }

            float minWidth = rootElement.resolvedStyle.minWidth.value;
            float maxWidth = rootElement.resolvedStyle.maxWidth.value;
            if (minWidth >= PatchedMinWidth &&
                maxWidth >= PatchedMaxWidth)
            {
                Plugin.Log.LogInfo(
                    $"Item Loot UI width ({minWidth} to {maxWidth}) was changed by the game devs or already patched");
                return;
            }

            rootElement.style.minWidth = new StyleLength(PatchedMinWidth);
            rootElement.style.maxWidth = new StyleLength(PatchedMaxWidth);
            rootElement.style.width = StyleKeyword.Auto;

#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(PContainerUI)}.{nameof(PContainerUI.OnFullyInitialized)} - Container width patched, removing patch");
#endif
        }
        finally
        {
            HarmonyInstance.UnpatchSelf();
        }
    }
}
