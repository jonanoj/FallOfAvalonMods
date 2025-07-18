using System;
using System.Reflection;
using Awaken.Utility.Collections;
using Awaken.Utility.Enums;
using Awaken.Utility.Enums.Helpers;

namespace AdditionalInventorySorting.Utils;

public static class RichEnumPatcher
{
    private static readonly OnDemandCache<string, object> InstanceCache;

    static RichEnumPatcher()
    {
        var cacheField = typeof(StaticStringSerialization).GetField("s_instanceCache",
            BindingFlags.Static | BindingFlags.NonPublic);
        if (cacheField == null)
        {
            Plugin.Log.LogError("Can't find enum cache, unable to add new enum members.");
            return;
        }

        if (cacheField.GetValue(null) is not OnDemandCache<string, object> onDemandCache)
        {
            Plugin.Log.LogError(
                $"Got unexpected enum cache type: {cacheField.FieldType}, unable to add new enum members.");
            return;
        }

        InstanceCache = onDemandCache;
    }

    public static bool AddOrUpdateMember<TEnum>(TEnum memberValue) where TEnum : RichEnum
    {
        if (InstanceCache == null)
            return false;

        string qualifiedName;
        try
        {
            qualifiedName = StaticStringSerialization.TypeName(typeof(TEnum));
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"{nameof(RichEnum)} not found in cache, not patching {typeof(TEnum)} - {ex}");
            return false;
        }

        string memberName = memberValue.EnumName;
        string instanceCacheKey = qualifiedName + ":" + memberName;
        if (InstanceCache.Contains(instanceCacheKey))
        {
            Plugin.Log.LogWarning($"{nameof(RichEnum)} member already exists, overriding. memberName={memberName}");
        }
        else
        {
            Plugin.Log.LogInfo($"Injecting {nameof(RichEnum)} member. memberName={memberName}");
        }

        InstanceCache[instanceCacheKey] = memberValue;
        return true;
    }
}
