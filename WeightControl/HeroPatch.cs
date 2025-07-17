using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Statuses;
using Awaken.TG.Main.Templates;
using Awaken.TG.MVC;
using HarmonyLib;

namespace WeightControl;

[HarmonyPatch]
public class HeroPatch
{
    private const string OverencumberedGuid = "61d1845f37ba56a4096a46408ce80ee3"; // Status_Overencumbered

    [HarmonyPatch(typeof(Hero), "OnFullyInitialized")]
    [HarmonyPostfix]
    public static void OnFullyInitializedPostfix(Hero __instance)
    {
        if (!TryGetTemplate(OverencumberedGuid, out StatusTemplate overencumberedTemplate))
        {
            return;
        }

        // Remove the overencumbered status if it exists, the game recalculates the encumbrance status after this patch and will re-apply if needed
        CharacterStatuses characterStatuses = __instance?.Statuses;
        Status status =
            characterStatuses?.AllStatuses.FirstOrDefault(status => status.Template == overencumberedTemplate);
        if (status != null)
        {
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(Hero)}.OnFullyInitialized: Overencumbered status exists, removing it");
#endif
            characterStatuses.RemoveStatus(overencumberedTemplate);
        }
#if DEBUG
        else
        {
            Plugin.Log.LogInfo(
                $"{nameof(Hero)}.OnFullyInitialized: Overencumbered status does not exist");
        }
#endif
    }

    private static bool TryGetTemplate<T>(string guid, out T template)
        where T : Template
    {
        TemplatesProvider templatesProvider = World.Services?.Get<TemplatesProvider>();
        if (templatesProvider == null)
        {
            Plugin.Log.LogWarning("TemplatesProvider is not available yet, can't find template details");
            template = null;
            return false;
        }

        template = templatesProvider.Get<T>(guid);
        if (template is null || !template)
        {
            Plugin.Log.LogWarning($"{typeof(T).Name} template with GUID {guid} not found.");
            return false;
        }

#if DEBUG
        Plugin.Log.LogInfo(
            $"Found {typeof(T).Name} template: {template.GUID} - {template.DebugName} - {template.name}");
#endif
        return true;
    }
}
