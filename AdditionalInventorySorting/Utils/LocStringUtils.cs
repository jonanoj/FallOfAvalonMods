using Awaken.TG.Main.Localization;

namespace AdditionalInventorySorting.Utils;

public static class LocStringUtils
{
    public static LocString New(string className, string memberName, string displayValue)
    {
        return new LocString { ID = SerializeLocStringID(className, memberName), Fallback = displayValue };
    }

    private static string SerializeLocStringID(string className, string memberName)
    {
        return className + "/" + memberName;
    }
}
