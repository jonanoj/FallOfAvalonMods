using Awaken.TG.Main.Localization;

namespace ImprovedInventory.Utils;

public static class LocStringUtils
{
    public static void Initialize(LocString locString, string className, string memberName, string displayValue)
    {
        locString.ID = SerializeLocStringID(className, memberName);
        locString.Fallback = displayValue;
    }

    private static string SerializeLocStringID(string className, string memberName)
    {
        return className + "/" + memberName;
    }
}
