using System.Diagnostics.CodeAnalysis;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Statuses;
using Awaken.TG.Main.Scenes.SceneConstructors;
using Awaken.TG.Main.Templates;
using Awaken.TG.MVC;
using HarmonyLib;

namespace HigherWeightLimit;

[HarmonyPatch]
public class HeroPatch
{
    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void OnFullyInitializedPostfix(Hero __instance)
    {
        TemplateReference templateReference = CommonReferences.Get?.OverEncumbranceStatus;
        if (templateReference == null)
        {
            Plugin.Log.LogWarning("OverEncumbranceStatus is null, can't remove");
            return;
        }

        StatusTemplate overEncumbranceStatus = templateReference.TryGet<StatusTemplate>();
        if (overEncumbranceStatus is null)
        {
            Plugin.Log.LogWarning("OverEncumbranceStatus template is null, can't remove");
            return;
        }

        // Remove the overencumbered status if it exists, the game recalculates the encumbrance status after this patch and will re-apply if needed
        CharacterStatuses characterStatuses = __instance?.Statuses;
        Status status = characterStatuses?.FirstFrom(overEncumbranceStatus);
        if (status != null)
        {
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(Hero)}.{nameof(Hero.OnFullyInitialized)}: Overencumbered status exists, removing it");
#endif
            characterStatuses.RemoveStatus(overEncumbranceStatus);
        }
#if DEBUG
        else
        {
            Plugin.Log.LogInfo(
                $"{nameof(Hero)}.{nameof(Hero.OnFullyInitialized)}: Overencumbered status does not exist");
        }
#endif
    }
}
