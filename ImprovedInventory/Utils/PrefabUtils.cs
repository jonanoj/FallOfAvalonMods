using System;
using System.IO;
using UnityEngine;

namespace ImprovedInventory.Utils;

public static class PrefabUtils
{
    // See: Assets/Code/MVC/World.cs
    // https://github.com/AR-Questline/merlin-workshop/blob/8a50e03ad4b4f7c73738e67e3bb49d2e62bffc3e/Assets/Code/MVC/World.cs#L677
    private const string PrefabsPath = "Prefabs/MapViews";

    public static GameObject TryGetPrefab(string prefabName)
    {
        string prefabPath = Path.Combine(PrefabsPath, prefabName);
        try
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Plugin.Log.LogError($"Prefab doesn't exist: {prefabPath}");
                return null;
            }

            return prefab;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Failed to load prefab: {prefabPath}: {ex}");
            return null;
        }
    }
}
