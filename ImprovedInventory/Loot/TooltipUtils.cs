using System.Linq;
using Awaken.Utility.GameObjects;
using ImprovedInventory.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ImprovedInventory.Loot;

public class Cached<T>
{
    public bool Loaded { get; set; }
    public T Value { get; set; }
}

public static class TooltipUtils
{
    private static GameObject ItemTooltipPrefab => PrefabUtils.TryGetPrefab("Items/TooltipSystem/VItemTooltipSystemUI");

    private static readonly Cached<GameObject> FooterContentCache = new();

    public static Texture GetWeightTexture()
    {
        return GetFooterTexture("Weight/Image");
    }

    public static Texture GetPriceTexture()
    {
        return GetFooterTexture("Price/Image");
    }

    private static Texture GetFooterTexture(string textureObjectPath)
    {
        GameObject footer = GetCachedFooterContent();
        return footer.transform.Find(textureObjectPath).TryGetComponent(out Image image) ? image.mainTexture : null;
    }

    private static GameObject GetCachedFooterContent()
    {
        if (FooterContentCache.Loaded)
        {
            return FooterContentCache.Value;
        }

        FooterContentCache.Loaded = true;
        FooterContentCache.Value = GetBaseFooterContent();
        return FooterContentCache.Value;
    }

    private static GameObject GetBaseFooterContent()
    {
        if (ItemTooltipPrefab == null)
        {
            Plugin.Log.LogError("ItemTooltipPrefab is null, cannot get footer");
            return null;
        }

        Transform footer = ItemTooltipPrefab.FindChildRecursively("Footer");
        if (footer == null)
        {
            Plugin.Log.LogError("Footer not found in ItemTooltipPrefab, can't get footer");
            return null;
        }

        Transform content = footer.Find("Content");
        if (content == null)
        {
            Plugin.Log.LogError("Content not found in ItemTooltipPrefab, can't get footer content");
            return null;
        }

        GameObject moddedObjectsRoot = GetModdedObjectsRoot();
        if (moddedObjectsRoot == null)
        {
            return null;
        }

        const string footerContentName = "ImprovedInventoryLootItemStats";
        GameObject footerContentClone = moddedObjectsRoot.transform.Find(footerContentName)?.gameObject;
        if (footerContentClone != null)
        {
            return footerContentClone;
        }

        footerContentClone = Object.Instantiate(content.gameObject, moddedObjectsRoot.transform);
        footerContentClone.name = footerContentName;
        footerContentClone.SetActive(false); // Don't render the clone, we just want to get the images

        // Remove the item category
        Transform typeName = footerContentClone.transform.Find("TypeName");
        if (typeName != null)
        {
            Object.Destroy(typeName.gameObject);
        }
        else
        {
            Plugin.Log.LogWarning("TypeName not found in footer content clone.");
        }


        if (!TryGetLabel(footerContentClone, "Weight/Weight") || !TryGetLabel(footerContentClone, "Price/Price"))
        {
            Plugin.Log.LogError("Failed detect price/weight elements in footer clone.");
            Object.Destroy(footerContentClone);
            return null;
        }

        return footerContentClone;
    }

    private static bool TryGetLabel(GameObject footer, string labelName)
    {
        return footer.transform.Find(labelName)?.TryGetComponent(out TextMeshProUGUI _) ?? false;
    }

    private static GameObject GetModdedObjectsRoot()
    {
        Scene applicationScene = SceneManager.GetSceneByName("ApplicationScene");
        if (!applicationScene.isLoaded)
        {
            Plugin.Log.LogError("ApplicationScene is doesn't exist, this should never happen");
            return null;
        }

        const string jonanojModded = "jonanoj.Modded";

        GameObject rootObject = applicationScene.GetRootGameObjects().FirstOrDefault(go => go.name == jonanojModded);
        if (rootObject == null)
        {
            rootObject = new GameObject(jonanojModded);
            SceneManager.MoveGameObjectToScene(rootObject, applicationScene);
        }

        return rootObject;
    }
}
