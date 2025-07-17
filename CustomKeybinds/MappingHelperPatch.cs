using System.Collections.Generic;
using System.Linq;
using Awaken.TG.Main.Utility;
using HarmonyLib;
using Rewired;

namespace CustomKeybinds;

[HarmonyPatch]
public class MappingHelperPatch
{
    [HarmonyPatch(typeof(ReInput.MappingHelper), nameof(ReInput.MappingHelper.UserAssignableMapCategories),
        MethodType.Getter)]
    [HarmonyPostfix]
    public static void MappingHelperUserAssignableMapCategoriesPostfix(ReInput.MappingHelper __instance,
        ref IEnumerable<InputMapCategory> __result)
    {
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(ReInput.MappingHelper)}.{nameof(ReInput.MappingHelper.UserAssignableMapCategories)} Postfix");
#endif
        IEnumerable<InputMapCategory> mapCategories = __instance.MapCategories;
        if (mapCategories == null)
        {
            Plugin.Log.LogError($"{nameof(ReInput.MappingHelper)}.{nameof(ReInput.MappingHelper.MapCategories)}" +
                                " is not an IEnumerable, can't patch keybinds");
            return;
        }

#if DEBUG
        Plugin.Log.LogInfo("All map categories:");
        foreach (InputMapCategory category in mapCategories.ToList())
        {
            Plugin.Log.LogInfo(
                $"Category - {category.name}, id={category.id}, descriptiveName={category.descriptiveName}");
        }

        Plugin.Log.LogInfo("Original map categories:");
        foreach (InputMapCategory category in __result.ToList())
        {
            Plugin.Log.LogInfo(
                $"Category - {category.name}, id={category.id}, descriptiveName={category.descriptiveName}");
        }
#endif

        if (Plugin.PluginConfig.IncludeDebugKeys.Value)
        {
            __result = mapCategories;
        }
        else
        {
            __result = mapCategories.Where(category => category.name != nameof(KeyBindings.Debug));
        }
    }
}
