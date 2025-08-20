using System;
using Awaken.TG.Main.Settings.Accessibility;
using Awaken.TG.MVC;
using TMPro;
using UnityEngine;

namespace DamageNumbers;

public class DamageNumber : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;
    private Camera _camera;
    private Canvas _canvas;
    private RectTransform _canvasRectTransform;
    private float _floatSpeed;
    private float _lifetime;
    private float _elapsed;
    private Vector3 _position;
    private Vector3 _floatDirection;
    private Color _startColor;
    private Action<DamageNumber> _onDespawn;

    private int _fontSize;

    public void Awake()
    {
        _rectTransform = gameObject.AddComponent<RectTransform>();
        _text = gameObject.AddComponent<TextMeshProUGUI>();
        _text.alignment = TextAlignmentOptions.Center;
        _text.textWrappingMode = TextWrappingModes.NoWrap;
        _text.raycastTarget = false;
    }

    private static int GetFontSize()
    {
        // Font size modified from accessibility setting, if available
        int fontSizeChange = World.Any<FontSizeSetting>()?.ActiveFontSize?.FontSizeChange ?? 0;

        return Plugin.PluginConfig.FontSize.Value + fontSizeChange;
    }

    public void Init(string damage, Vector3 position, Color color, Action<DamageNumber> onDespawnCallback)
    {
        _camera = Camera.main;
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            throw new InvalidOperationException("Canvas not found");
        }

        _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        if (_canvasRectTransform == null)
        {
            throw new InvalidOperationException("RectTransform not found on Canvas");
        }

        _fontSize = GetFontSize();
        _startColor = color;

        _text.text = damage;
        _text.color = color;
        _text.fontSize = _fontSize;

        _position = position;
        float maxAngle = Plugin.PluginConfig.MaximumFloatAngle.Value;
        float angle = UnityEngine.Random.Range(-maxAngle, maxAngle);
        _floatDirection = Quaternion.Euler(0, 0, angle) * Vector3.up;
        _lifetime = Plugin.PluginConfig.TextFadeTimeSeconds.Value;
        _floatSpeed = Plugin.PluginConfig.TextFloatSpeed.Value;
        _elapsed = 0f;
        _onDespawn = onDespawnCallback;
        gameObject.SetActive(true);
    }

    private void ResetDamageNumber()
    {
        _text.text = string.Empty;
        _text.color = Color.clear;
        _rectTransform.localPosition = Vector3.zero;
        _elapsed = 0f;
        _lifetime = 0f;
        _onDespawn = null;
    }

    public void Update()
    {
        _elapsed += Time.deltaTime;

        if (_camera && _canvas && _canvasRectTransform)
        {
            Vector3 worldPos = _position + _floatDirection * (_elapsed * _floatSpeed);
            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPos, null,
                out Vector2 localPoint);
            _rectTransform.localPosition = localPoint;
        }

        _text.color = new Color(_startColor.r, _startColor.g, _startColor.b,
            _startColor.a * (1 - _elapsed / _lifetime));

        if (_elapsed >= _lifetime)
        {
#if DEBUG
            Plugin.Log.LogDebug($"DamageNumber returned to pool {_elapsed} > {_lifetime}");
#endif
            _onDespawn?.Invoke(this);
            gameObject.SetActive(false);
            ResetDamageNumber();
        }
    }
}
