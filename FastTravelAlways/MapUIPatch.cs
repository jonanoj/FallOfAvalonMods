using Awaken.TG.Main.Heroes.CharacterSheet.Map;
using HarmonyLib;

namespace FastTravelAlways;

[HarmonyPatch(typeof(MapUI), "OnInitialize")]
public class MapUIPatch
{
    public static void Postfix(MapUI __instance)
    {
        __instance?.AllowFastTravel();
    }
}
