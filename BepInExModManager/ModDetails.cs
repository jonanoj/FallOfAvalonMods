using System.Collections.Generic;
using System.Linq;
using Awaken.Utility.Assets.Modding;
using BepInEx.Bootstrap;

namespace BepInExModManager;

public static class ModDetails
{
    public static List<ModMetadata> GetAllLoadedMods()
    {
        return Chainloader.PluginInfos
            .Select(plugin => plugin.Value.Metadata)
            .Select(metadata => new ModMetadata
            {
                name = metadata.Name ?? metadata.GUID,
                author = ExtractModAuthor(metadata.GUID),
                version = metadata.Version?.ToString(),
                tags = ["BepInEx"]
            })
            .ToList();
    }

    private static string ExtractModAuthor(string guid)
    {
        if (string.IsNullOrEmpty(guid))
            return "Unknown Author";

        // BepInEx GUID is usually formatted as "com.author.name.ModName"
        int lastDotIndex = guid.LastIndexOf('.');
        return lastDotIndex > 0
            ? guid[..lastDotIndex]
            : guid;
    }
}
