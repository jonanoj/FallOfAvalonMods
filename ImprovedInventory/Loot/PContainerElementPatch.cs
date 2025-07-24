using System;
using System.Globalization;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Locations.Containers;
using EnhydraGames.BetterTextOutline;
using HarmonyLib;
using ImprovedInventory.Inventory.Sorting;
using UnityEngine.UIElements;

namespace ImprovedInventory.Loot;

public class PContainerElementPatch
{
    private static readonly Harmony HarmonyInstance = new(nameof(PContainerElementPatch));

    private const string VisualElementName = "jonanoj-ImprovedInventory-item-stats";
    private const string PriceElementName = "jonanoj-ImprovedInventory-item-price";
    private const string WeightElementName = "jonanoj-ImprovedInventory-item-weight";

    public static void Patch()
    {
        HarmonyInstance.PatchAll(typeof(PContainerElementPatch));
    }

    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }

    [HarmonyPatch(typeof(PContainerElement), nameof(PContainerElement.CacheVisualElements))]
    [HarmonyPostfix]
    public static void PContainerElementCacheVisualElements(PContainerElement __instance)
    {
        var priceStats = new VisualElement
        {
            style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, marginLeft = 10 }
        };
        priceStats.Add(new BetterOutlinedLabel { name = PriceElementName });
        priceStats.Add(new Image
        {
            image = TooltipUtils.GetPriceTexture(),
            name = "PriceIcon",
            style = { width = 16, height = 16, marginLeft = 6 }
        });

        var weightStats = new VisualElement
        {
            style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, marginLeft = 10 }
        };
        weightStats.Add(new BetterOutlinedLabel { name = WeightElementName });
        weightStats.Add(new Image
        {
            image = TooltipUtils.GetWeightTexture(),
            name = "WeightIcon",
            style = { width = 16, height = 16, marginLeft = 6 }
        });

        var extraItemStats = new VisualElement
        {
            name = VisualElementName,
            style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, marginRight = 20 }
        };
        extraItemStats.Add(priceStats);
        extraItemStats.Add(weightStats);

        BetterOutlinedLabel quantity = __instance._quantityLabel;
        VisualElement allItemStats = quantity.parent;
        // Add the price data before the quantity label
        allItemStats.Insert(allItemStats.IndexOf(quantity), extraItemStats);
    }

    [HarmonyPatch(typeof(PContainerElement), nameof(PContainerElement.SetData))]
    [HarmonyPostfix]
    public static void PContainerElementSetDataPostfix(PContainerElement __instance, [HarmonyArgument(1)] Item item)
    {
        VisualElement root = __instance._quantityLabel.parent;
        BetterOutlinedLabel price = root.Q<BetterOutlinedLabel>(PriceElementName);
        price.text = PriceLabel(item);
        BetterOutlinedLabel weight = root.Q<BetterOutlinedLabel>(WeightElementName);
        weight.text = MathF.Round(item.Weight, 2).ToString(CultureInfo.InvariantCulture);
    }

    private static string PriceLabel(Item item)
    {
        if (!Plugin.PluginConfig.ShowWorthInLoot.Value)
        {
            return item.Price.ToString();
        }

        // Round to 0 digits to add as little width as possible
        string worth = ExtendedItemComparers.GetValueToRatioString(item.Price, item.Weight, 0);
        return $"({worth}) {item.Price}";
    }
}
