using System;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Locations.Containers;
using Awaken.TG.Main.UIToolkit;
using HarmonyLib;
using Il2CppSystem.Linq;
using UnityEngine.UIElements;

namespace AdditionalInventorySorting;

[HarmonyPatch]
public class PContainerPatch
{
    private const float DefaultMaxWidth = 600f;
    private const float PatchedMaxWidth = DefaultMaxWidth * 1.5f;

    [HarmonyPatch(typeof(PContainerUI), nameof(PContainerUI.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void PContainerUIOnFullyInitializedPostfix(PContainerUI __instance)
    {
        VisualElement rootElement = __instance.Content.Children()
            .FirstOrDefault(new Func<VisualElement, bool>(child => child.name == "root"));
        if (rootElement == null)
        {
            Plugin.Log.LogWarning("Couldn't find item pickup UI element");
            return;
        }

#if DEBUG
        LogVisualElementWidth("Before", rootElement);
#endif

        if (rootElement.m_Style.maxWidth.value >= PatchedMaxWidth)
        {
#if DEBUG
            Plugin.Log.LogInfo("Existing UI element is already large enough");
#endif
            return;
        }

        rootElement.style.maxWidth = new StyleLength(PatchedMaxWidth);
        rootElement.style.width = StyleKeyword.Auto;

#if DEBUG
        LogVisualElementWidth("After", rootElement);
#endif
    }

#if DEBUG
    private static void LogVisualElementWidth(string name, VisualElement element)
    {
        Plugin.Log.LogInfo($"{name} - {element.name} style={Style(element.style)}");
        Plugin.Log.LogInfo($"{name} - {element.name} resolvedStyle={Style(element.resolvedStyle)}");
        Plugin.Log.LogInfo($"{name} - {element.name} computedStyle={Style(element.m_Style)}");
    }

    private static string Style(ComputedStyle style) =>
        $"width={style.width}, minWidth={style.minWidth}, maxWidth={style.maxWidth}";

    private static string Style(IResolvedStyle style) =>
        $"width={style.width}, minWidth={style.minWidth}, maxWidth={style.maxWidth}";

    private static string Style(IStyle style) =>
        $"width={style.width}, minWidth={style.minWidth}, maxWidth={style.maxWidth}";
#endif

    [HarmonyPatch(typeof(PContainerElement), nameof(PContainerElement.SetData))]
    [HarmonyPostfix]
    public static void PContainerElementSetDataPostfix(PContainerElement __instance, [HarmonyArgument(1)] Item item)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(PContainerElement)}.{nameof(PContainerElement.SetData)} - " +
                           $"itemLabel={__instance._nameLabel.text}, quantityLabel={__instance._quantityLabel.text}");
#endif

        string priceLabel = ShortPriceLabel(item);
        if (!__instance._quantityLabel.IsActive())
        {
            __instance._quantityLabel.text = priceLabel;
            __instance._quantityLabel.SetActiveOptimized(true);
        }
        else
        {
            __instance._quantityLabel.text += " " + priceLabel;
        }
    }

    private static string ShortPriceLabel(Item item)
    {
        // No point to display weight ratio of 0 weight items
        if (item.Weight == 0)
        {
            return $"({item.Price}$)";
        }

        // Round to 0 digits to save as much width as possible
        return
            $"({item.Price}$/{item.Weight}kg = {ExtendedItemComparers.GetPriceToWeightRatioString(item.Price, item.Weight, 0)})";
    }
}
