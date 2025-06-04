using Awaken.TG.Graphics.DayNightSystem;
using HarmonyLib;

namespace DisableNightGlow;

[HarmonyPatch(typeof(HeroWyrdNightEdge), nameof(HeroWyrdNightEdge.Execute))]
public class HeroWyrdNightEdgePatch
{
    public static bool Prefix(HeroWyrdNightEdge __instance)
    {
#if DEBUG
        Plugin.Log.LogInfo(
            $"{nameof(HeroWyrdNightEdge)}.{nameof(HeroWyrdNightEdge.Execute)} called, enabled={__instance.enabled}, disabling");
#endif
        __instance.enabled = false;
        return true;
    }
}
