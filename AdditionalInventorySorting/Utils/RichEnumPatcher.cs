using Awaken.Utility.Enums;
using Awaken.Utility.Enums.Helpers;

namespace AdditionalInventorySorting.Utils;

public static class RichEnumPatcher
{
    public static bool AddOrUpdateMember<TEnum>(TEnum memberValue) where TEnum : RichEnum
    {
        if (!StaticStringSerialization.s_qualifiedNameByType.TryGetValue(memberValue.GetIl2CppType(),
                out string qualifiedName))
        {
            Plugin.Log.LogWarning($"{nameof(RichEnum)} not found in cache, not patching {typeof(TEnum)}");
            return false;
        }

        string memberName = memberValue.EnumName;
        string instanceCacheKey = qualifiedName + ":" + memberName;

        if (StaticStringSerialization.s_instanceCache.Contains(instanceCacheKey))
        {
            Plugin.Log.LogWarning($"{nameof(RichEnum)} member already exists, overriding. memberName={memberName}");
        }
        else
        {
            Plugin.Log.LogInfo($"Injecting {nameof(RichEnum)} member. memberName={memberName}");
        }

        StaticStringSerialization.s_instanceCache[instanceCacheKey] = memberValue;
        return true;
    }
}
