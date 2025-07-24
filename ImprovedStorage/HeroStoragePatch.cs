using System.Collections.Generic;
using System.Linq;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes.Storage;
using Awaken.TG.Main.UI.ButtonSystem;
using Awaken.TG.Main.Utility;
using Awaken.TG.MVC;
using HarmonyLib;

namespace ImprovedStorage;

[HarmonyPatch]
public class HeroStoragePatch
{
    [HarmonyPatch(typeof(HeroStorageUI), nameof(HeroStorageUI.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void HeroStorageUIOnFullyInitializedPostfix(HeroStorageUI __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(HeroStorageUI)}.{nameof(HeroStorageUI.OnFullyInitialized)}");
#endif
        var transferStackPrompt = Prompt.Tap(KeyBindings.UI.Items.UnequipItem,
            Plugin.LanguageConfig.TransferStackDisplayName.Value,
            () => TransferStack(__instance.TabsController.CurrentTab));

        var transferAllPrompt = Prompt.Hold(KeyBindings.UI.Items.TransferItems,
            Plugin.LanguageConfig.TransferAllDisplayName.Value,
            () => TransferAll(__instance.TabsController.CurrentTab),
            holdTime: 1f);

        __instance.Prompts.AddPrompt(transferStackPrompt, __instance);
        __instance.Prompts.AddPrompt(transferAllPrompt, __instance);
        __instance.Prompts.RefreshPromptsPositions();
    }

    private static void TransferStack(HeroStorageTabUI currentTab)
    {
        if (currentTab.HoveredItem == null)
            return;

        currentTab.SelectItem(currentTab.HoveredItem, currentTab.HoveredItem.Quantity);
    }

    private static void TransferAll(HeroStorageTabUI currentTab)
    {
        ItemsTabType itemTab = currentTab.ItemsUI.CurrentType;
        List<Item> items = currentTab.Items
            .Where(item => currentTab is HeroStoragePutUI
                ? IsImportantItem(item) && itemTab.Contains(item) // Avoid transferring equipped items
                : itemTab.Contains(item))
            .ToList();

        Plugin.Log.LogInfo(
            $"Moving {items.Count} items to {currentTab.ActionName} (in/from) storage - tabType={itemTab.EnumName}");

        foreach (Item item in items)
        {
            item.MoveTo(currentTab.InventoryTo, item.Quantity);
            if (currentTab.SellerNoLongerOwnsItem(item))
            {
                currentTab.ItemsUI.GetItemsListElementWithItem(item)?.Discard();
            }
        }

        currentTab.ItemsUI.Trigger(ItemsUI.Events.ItemsCollectionChanged, currentTab.Items);
        currentTab.ItemsUI.FullRefresh();
    }

    private static bool IsImportantItem(Item item)
    {
        return !item.IsEquipped && !item.IsQuestItem && !item.IsUsedInLoadout();
    }
}
