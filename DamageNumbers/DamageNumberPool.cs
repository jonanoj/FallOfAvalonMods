using System.Collections.Generic;
using Awaken.TG.Main.Fights.DamageInfo;
using Awaken.TG.Main.UI.HUD;
using Awaken.TG.MVC;
using UnityEngine;

namespace DamageNumbers;

public class DamageNumberPool : MonoBehaviour
{
    private readonly Queue<DamageNumber> _pool = new(Plugin.PluginConfig.MaximumDamageNumbers.Value);
    private Canvas _damageNumberCanvas;

    public void Start()
    {
        HUD hud = World.Only<HUD>();

        const string gameObjectName = "DamageNumberCanvas";
        Transform previous = hud._uguiHUDRoot.Find(gameObjectName);
        if (previous != null)
        {
            // Clean up any previous instance of DamageNumberCanvas
            Destroy(previous.gameObject);
        }

        // Create DamageNumberCanvas under HUD
        var canvasGameObject = new GameObject(gameObjectName);
        canvasGameObject.transform.SetParent(hud._uguiHUDRoot, false);
        _damageNumberCanvas = canvasGameObject.AddComponent<Canvas>();
        _damageNumberCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        for (int i = 0; i < Plugin.PluginConfig.MaximumDamageNumbers.Value; i++)
        {
            var damageNumber = new GameObject("DamageNumber " + i);
            damageNumber.transform.SetParent(_damageNumberCanvas.transform);
            DamageNumber script = damageNumber.AddComponent<DamageNumber>();
            damageNumber.SetActive(false);
            _pool.Enqueue(script);
        }
    }

    public void ShowDamage(DamageOutcome outcome)
    {
        float amount = Mathf.Round(outcome.Damage.Amount * 10f) / 10f;
        if (amount < Plugin.PluginConfig.HideDamageNumberThreshold.Value)
        {
            return;
        }

        string amountString = amount >= 1 ? Mathf.RoundToInt(amount).ToString() : amount.ToString("N1");

        if (_pool.Count == 0)
        {
            Plugin.Log.LogWarning(
                $"Too many damage numbers in use, skipping {amountString} damage. " +
                $"Increase the {nameof(Plugin.PluginConfig.MaximumDamageNumbers)} value in the config file to see more.");
            return;
        }


        Vector3 position = outcome.Position;
        if (outcome.HitCollider == null)
        {
            // Damage over time has no collider, and it uses the enemy's foot position,
            // so we raise the position slightly up
            position += Vector3.up * 1.5f;
        }

#if DEBUG
        Plugin.Log.LogInfo($"Showing damage number at {position} with amount {amountString}");
#endif

        Color color = outcome.DamageModifiersInfo.AnyCritical
            ? Plugin.PluginConfig.CriticalDamageColor.Value
            : Plugin.PluginConfig.DamageColor.Value;

        DamageNumber dmgNum = _pool.Dequeue();
        dmgNum.Init(amountString, position, color, ReturnToPool);
    }

    private void ReturnToPool(DamageNumber dmgNum)
    {
        _pool.Enqueue(dmgNum);
    }
}
