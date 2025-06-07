using Awaken.TG.Main.UI.ButtonSystem;
using Awaken.TG.Main.Utility;
using HarmonyLib;

namespace LessHoldTime;

[HarmonyPatch(typeof(Prompt), nameof(Prompt.Hold))]
public class PromptHoldPatch
{
    public static void Postfix(Prompt __result)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(Prompt)}.{nameof(Prompt.Hold)} created. " +
                           $"keybind={__result._key.ToString()}, name={__result._name}," +
                           $"holdTime={__result.HoldTime}, actionName={__result.ActionName}");
#endif

        if (__result._key.CompareTo(KeyBindings.UI.Crafting.CraftOne) == 0)
        {
            float originalHoldTime = __result.HoldTime;
            float newHoldTime = originalHoldTime * Plugin.PluginConfig.CraftHoldTimeMultiplier.Value;

            if (Plugin.PluginConfig.CraftHoldTimeMultiplier.Value <= 0.10f)
            {
                // Change the UI from hold to tap if it's under a certain threshold
                __result._pressType = IButton.PressType.Tap;
            }

            __result.HoldTime = newHoldTime;
#if DEBUG
            Plugin.Log.LogInfo(
                $"Setting {nameof(KeyBindings.UI.Crafting.CraftOne)} - {__result.ActionName} prompt to {newHoldTime} (was {originalHoldTime})");
#endif
        }
#if DEBUG
        else
        {
            Plugin.Log.LogInfo($"{nameof(Prompt.Hold)} - Got new Prompt action, HoldTime={__result.HoldTime}, " +
                               $"name={__result._name}, keybind={__result._key.ToString()}");
        }
#endif
    }
}
