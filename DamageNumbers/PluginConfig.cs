using BepInEx.Configuration;
using UnityEngine;

namespace DamageNumbers;

public class PluginConfig
{
    public ConfigEntry<int> FontSize { get; private set; }
    public ConfigEntry<float> TextFadeTimeSeconds { get; private set; }
    public ConfigEntry<float> TextFloatSpeed { get; private set; }
    public ConfigEntry<Color> DamageColor { get; private set; }
    public ConfigEntry<Color> CriticalDamageColor { get; private set; }
    public ConfigEntry<int> MaximumDamageNumbers { get; private set; }
    public ConfigEntry<float> HideDamageNumberThreshold { get; private set; }

    private const string ColorDescription =
        """
        Format is RGBA - red, green, blue, alpha (opacity).
        All values are hexadecimal, ranging from 00 to FF. (0 to 255)
        Alpha value FF is fully visible, 00 is fully transparent.
        """;

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string damageNumbersCategory = "DamageNumbers";
            FontSize = config.Bind(damageNumbersCategory, nameof(FontSize), 32,
                "Font size for damage numbers. Adjust based on your screen resolution and personal preference");
            TextFadeTimeSeconds = config.Bind(damageNumbersCategory, nameof(TextFadeTimeSeconds), 2f,
                "Time in seconds before damage numbers fade out");
            TextFloatSpeed = config.Bind(damageNumbersCategory, nameof(TextFloatSpeed), 0.5f,
                "Speed at which the damage numbers float upwards. Higher values make them float faster");
            DamageColor = config.Bind(damageNumbersCategory, nameof(DamageColor), Color.yellow,
                $"""
                 Color of normal damage numbers. Use a color picker to select your preferred color.

                 {ColorDescription}
                 """);
            CriticalDamageColor = config.Bind(damageNumbersCategory, nameof(CriticalDamageColor),
                new Color(1f, 0.5f, 0, 1f),
                $"""
                 Color of critical damage numbers. Use a color picker to select your preferred color.

                 {ColorDescription}
                 """);
            MaximumDamageNumbers = config.Bind(damageNumbersCategory, nameof(MaximumDamageNumbers), 50,
                """
                Maximum number of damage numbers to display at once.
                Increase if you want to see more damage numbers simultaneously, but it may impact readability or performance if set too high"
                """);
            HideDamageNumberThreshold = config.Bind(damageNumbersCategory, nameof(HideDamageNumberThreshold), 1f,
                """
                Minimum amount of damage to show a number for, any value below this will be hidden
                You can also specify number
                """);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
