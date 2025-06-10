using System.Diagnostics.CodeAnalysis;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Templates;
using Awaken.TG.MVC;
using HarmonyLib;
using Il2CppSystem.Linq;

namespace UnlimitedOriginPotions;

[HarmonyPatch]
public class HeroPatch
{
    private const string OriginPotionGuid = "70950703d3587b347a933b46c4e65871"; // ItemTemplate_Alchemy_RespecPotion

    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void OnInitializePostfix(Hero __instance)
    {
        if (!TryGetItemTemplate(OriginPotionGuid, out ItemTemplate originPotionTemplate))
        {
            return;
        }

        Item firstItem = __instance.HeroItems.Inventory.FirstOrDefault();
        if (firstItem == null)
        {
            Plugin.Log.LogWarning("Hero's inventory is empty. Cannot add potion.");
            return;
        }

        // TODO: find a better way to get the IInventory, for some reason Hero.Inventory is a separate ICharacterInventory interface, and you can't cast it to IInventory
        IInventory inventory = firstItem.Inventory;

        float currentPotions = inventory.NumberOfItems(originPotionTemplate);
        if (currentPotions >= 10)
        {
            Plugin.Log.LogInfo(
                $"Player already has {currentPotions} {originPotionTemplate.ItemName}, not adding more.");
            return;
        }

        int missing = 10 - (int)currentPotions;
        Plugin.Log.LogInfo($"Found {currentPotions} {originPotionTemplate.ItemName}, adding {missing}.");
        originPotionTemplate.ChangeQuantity(inventory, missing);
    }

    private static bool TryGetItemTemplate(string guid, [MaybeNullWhen(false)] out ItemTemplate originPotionTemplate)
    {
        TemplatesProvider templatesProvider = World.Services?.Get<TemplatesProvider>();
        if (templatesProvider == null)
        {
            Plugin.Log.LogWarning("TemplatesProvider is not available yet, can't find item details");
            originPotionTemplate = null;
            return false;
        }

        originPotionTemplate = templatesProvider.Get<ItemTemplate>(guid);
        if (originPotionTemplate == null)
        {
            Plugin.Log.LogWarning($"Item template with GUID {guid} not found.");
            return false;
        }

#if DEBUG
        Plugin.Log.LogInfo($"Found item template: {originPotionTemplate.GUID} - {originPotionTemplate.ItemName}");
#endif
        return true;
    }
}
