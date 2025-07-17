using Awaken.TG.Main.UI.ButtonSystem;
using Awaken.TG.Main.Utility;
using Awaken.TG.Main.Utility.UI.Keys;
using HarmonyLib;

namespace LessHoldTime;

[HarmonyPatch]
public class PromptPatch
{
    [HarmonyPatch(typeof(Prompt), MethodType.Constructor, [
        typeof(KeyBindings),
        typeof(string),
        typeof(IButton.PressType),
        typeof(System.Action),
        typeof(Prompt.Position),
        typeof(ControlSchemeFlag),
        typeof(float)
    ])]
    [HarmonyPrefix]
    public static void PromptConstructorPrefix(
        [HarmonyArgument(0)] KeyBindings key,
        [HarmonyArgument(1)] string name,
        [HarmonyArgument(2)] ref IButton.PressType pressType,
        [HarmonyArgument(6)] ref float holdTime)
    {
        if (pressType != IButton.PressType.Hold)
        {
            return;
        }

#if DEBUG
        Plugin.Log.LogInfo($"{nameof(Prompt)}.{nameof(Prompt.Hold)} created. " +
                           $"keybind={key}, name={name}, holdTime={holdTime}");
#endif

        if (key.CompareTo(KeyBindings.UI.Crafting.CraftOne) == 0)
        {
            float originalHoldTime = holdTime;
            float newHoldTime = originalHoldTime * Plugin.PluginConfig.CraftHoldTimeMultiplier.Value;

            if (Plugin.PluginConfig.CraftHoldTimeMultiplier.Value <= 0.10f)
            {
                // Change the UI from hold to tap if it's under a certain threshold
                pressType = IButton.PressType.Tap;
            }

            holdTime = newHoldTime;
#if DEBUG
            Plugin.Log.LogInfo(
                $"Setting {nameof(KeyBindings.UI.Crafting.CraftOne)} - {name} prompt to {newHoldTime} (was {originalHoldTime})");
#endif
        }
#if DEBUG
        else
        {
            Plugin.Log.LogInfo($"{nameof(Prompt.Hold)} - Got new Prompt action, HoldTime={holdTime}, " +
                               $"name={name}, keybind={key}");
        }
#endif
    }
}
