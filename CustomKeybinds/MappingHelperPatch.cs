using Awaken.TG.Main.Utility;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Linq;
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
        var mapCategories = __instance.MapCategories.TryCast<IEnumerable<InputMapCategory>>();
        if (mapCategories == null)
        {
            Plugin.Log.LogError($"{nameof(ReInput.MappingHelper)}.{nameof(ReInput.MappingHelper.MapCategories)}" +
                                " is not an IEnumerable, can't patch keybinds");
            return;
        }

#if DEBUG
        Plugin.Log.LogInfo("All map categories:");
        foreach (var category in mapCategories.ToList())
        {
            Plugin.Log.LogInfo(
                $"Category - {category.name}, id={category.id}, descriptiveName={category.descriptiveName}");
        }

        Plugin.Log.LogInfo("Original map categories:");
        foreach (var category in __result.ToList())
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
            __result = mapCategories.Where(
                DelegateSupport.ConvertDelegate<Func<InputMapCategory, bool>>((InputMapCategory category) =>
                    category.name != nameof(KeyBindings.Debug)));
        }
    }
}
