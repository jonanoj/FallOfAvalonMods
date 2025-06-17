using System;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Templates;
using Awaken.TG.MVC;
using HarmonyLib;
using Il2CppSystem.Linq;

namespace WeightControl;

[HarmonyPatch]
public class ItemWeightPatch
{
    [HarmonyPatch(typeof(Item), nameof(Item.Weight), MethodType.Getter)]
    [HarmonyPrefix]
    public static bool ItemWeightPrefix(Item __instance, ref float __result)
    {
        if ((__instance.IsEquippable && !__instance.IsEquipped && Plugin.PluginConfig.DisableUnequipped.Value) ||
            (__instance.IsArrow && Plugin.PluginConfig.DisableArrows.Value) ||
            (__instance.IsPotion && Plugin.PluginConfig.DisablePotions.Value))
        {
            __result = 0f;
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPrefix]
    public static void HeroOnFullyInitializedPrefix()
    {
        TemplatesProvider templatesProvider = World.Services.Get<TemplatesProvider>();
        if (templatesProvider == null)
        {
            Plugin.Log.LogError("Couldn't find templates provider, can't reset item weight");
            return;
        }

        DateTime startTime = DateTime.Now;
        var items = templatesProvider.GetAllOfType<ItemTemplate>().ToList();
        Plugin.Log.LogInfo($"Patching {items.Count} item templates");
        foreach (ItemTemplate item in items)
        {
            if ((item.IsAlchemyComponent && Plugin.PluginConfig.DisableAlchemyComponents.Value)
                || ((item.IsConsumable) && Plugin.PluginConfig.DisableConsumables.Value)
                || (item.IsPlainFood && Plugin.PluginConfig.DisablePlainFood.Value)
                || ((item.IsPotion || item.IsBuffApplier) && Plugin.PluginConfig.DisablePotions.Value)
                || (item.IsCraftingComponent && Plugin.PluginConfig.DisableCraftingComponents.Value)
                || (item.IsRecipe && Plugin.PluginConfig.DisableRecipes.Value)
                || (item.IsReadable && Plugin.PluginConfig.DisableReadables.Value)
                || (item.IsGem && Plugin.PluginConfig.DisableRelics.Value)
                || (item.IsArrow && Plugin.PluginConfig.DisableArrows.Value)
                || (item.IsTool && Plugin.PluginConfig.DisableTools.Value)
                || (Plugin.PluginConfig.DisableOther.Value && (item.IsQuestItem() || item.IsOther())))
            {
                item.weight = 0f;
            }
        }

        TimeSpan patchTime = DateTime.Now - startTime;
        Plugin.Log.LogInfo($"Took {patchTime.TotalSeconds} seconds to patch all templates");
    }
}
