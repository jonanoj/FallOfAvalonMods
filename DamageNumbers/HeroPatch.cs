using System;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.MVC;
using Awaken.TG.MVC.Events;
using HarmonyLib;
using Unity.VisualScripting;

namespace DamageNumbers;

[HarmonyPatch]
public class HeroPatch
{
    private static IEventListener Listener { get; set; }

    [HarmonyPatch(typeof(Hero), nameof(Hero.OnFullyInitialized))]
    [HarmonyPostfix]
    public static void OnFullyInitializedPostfix()
    {
        // World.EventSystem and HUD aren't initialized when the mod is loaded,
        // so this patch is used to ensure they are loaded
        if (Listener != null)
        {
            World.EventSystem.RemoveListener(Listener);
        }

        Plugin.Log.LogInfo($"{nameof(Hero)}.{nameof(Hero.OnFullyInitialized)} called, setting up damage number pool");
        if (!Plugin.Instance.TryGetComponent(out DamageNumberPool damageNumberPool))
        {
            damageNumberPool = Plugin.Instance.AddComponent<DamageNumberPool>();
        }

        Listener = World.EventSystem.ListenTo(EventSelector.AnySource,
            HealthElement.Events.OnDamageDealt,
            outcome =>
            {
                if (outcome.Target == Hero.Current)
                {
                    return;
                }

                try
                {
                    damageNumberPool.ShowDamage(outcome);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to show damage number: {ex}");
                }
            });
    }

    public static void Dispose()
    {
        if (Listener != null)
        {
            World.EventSystem.RemoveListener(Listener);
            Listener = null;
        }
    }
}
