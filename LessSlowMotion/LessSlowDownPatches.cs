using System;
using Awaken.TG.Main.Fights.Modifiers;
using Awaken.TG.Main.Heroes.Combat;
using HarmonyLib;

namespace LessSlowMotion;

[HarmonyPatch]
public class LessSlowDownPatches
{
    private static DateTime LastUnskippedKillCameraTime = DateTime.MinValue;
    private static bool CallerDidntSkip;

    [HarmonyPatch(typeof(HeroCameraShakes), nameof(HeroCameraShakes.MeleeSlowDownTime))]
    [HarmonyPrefix]
    public static bool MeleeSlowDownTimePrefix(HeroCameraShakes __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(HeroCameraShakes)}.{nameof(HeroCameraShakes.MeleeSlowDownTime)} called, " +
                           $"_lastStealthKillCamera={__instance._lastStealthKillCamera}");
#endif
        bool shouldSkip = ShouldSkip(Plugin.PluginConfig.MinimumKillCameraCooldownSecs.Value);
        CallerDidntSkip = !shouldSkip;
        return !shouldSkip;
    }

    [HarmonyPatch(typeof(HeroCameraShakes), nameof(HeroCameraShakes.RangedSlowDownTime))]
    [HarmonyPrefix]
    public static bool RangedSlowDownTimePrefix(HeroCameraShakes __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(HeroCameraShakes)}.{nameof(HeroCameraShakes.RangedSlowDownTime)} called, " +
                           $"_lastStealthKillCamera={__instance._lastStealthKillCamera}");
#endif
        bool shouldSkip = ShouldSkip(Plugin.PluginConfig.MinimumKillCameraCooldownSecs.Value);
        CallerDidntSkip = !shouldSkip;
        return !shouldSkip;
    }


    [HarmonyPatch(typeof(SlowDownTime), nameof(SlowDownTime.OnInitialize))]
    [HarmonyPostfix]
    public static void SlowDownTimeOnInitializePostfix(SlowDownTime __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo($"{nameof(SlowDownTime)}.{nameof(SlowDownTime.OnInitialize)} called, " +
                           $"globalSourceID={__instance.GlobalSourceID}");
#endif
        // MeleeSlowDownTime and RangedSlowDownTime already handled the skip logic
        // in their prefixes, so we don't need to do anything here.
        // This function can also be called from other places (i.e. backstab)
        bool callerDidntSkip = CallerDidntSkip;
        CallerDidntSkip = false;
        if (callerDidntSkip)
        {
#if DEBUG
            Plugin.Log.LogInfo(
                $"{nameof(SlowDownTime)}.{nameof(SlowDownTime.OnInitialize)} called, but caller didn't skip. " +
                $"GlobalSourceID={__instance.GlobalSourceID}");
#endif
            return;
        }

        if (ShouldSkip(Plugin.PluginConfig.MinimumSlowMotionCooldownSecs.Value))
        {
            __instance.ReduceTime(100);
        }
    }

    private static bool ShouldSkip(int cooldown)
    {
        double secondsSinceLastKillCamera = (DateTime.Now - LastUnskippedKillCameraTime).TotalSeconds;
        bool shouldSkip = secondsSinceLastKillCamera < cooldown;

        if (!shouldSkip)
        {
            LastUnskippedKillCameraTime = DateTime.Now;
        }

#if DEBUG
        Plugin.Log.LogInfo($"ShouldSkip called, secondsSinceLastKillCamera={secondsSinceLastKillCamera}, " +
                           $"shouldSkip={shouldSkip}, cooldown={cooldown}");
        Plugin.Log.LogInfo($"Stack Trace: {Environment.StackTrace}");
#endif
        return shouldSkip;
    }
}
