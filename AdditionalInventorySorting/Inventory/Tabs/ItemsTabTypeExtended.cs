using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Awaken.TG.Main.Heroes.Items;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace AdditionalInventorySorting.Inventory.Tabs;

public static class ItemsTabTypeExtended
{
    public static readonly ItemsTabType Unread = New(nameof(Unread), CheckRead(false),
        Plugin.LanguageConfig.ReadableSubTypeUnreadDisplayName.Value);

    public static readonly ItemsTabType Read = New(nameof(Read), CheckRead(true),
        Plugin.LanguageConfig.ReadableSubTypeReadDisplayName.Value);

    private static ItemsTabType New(string name, Func<Item, bool> filter, string displayName) =>
        new(name, filter) { _title = LocStringUtils.New(nameof(ItemsTabTypeExtended), name, displayName) };


    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember(Unread) &&
               RichEnumPatcher.AddOrUpdateMember(Read);
    }

    private static Func<Item, bool> CheckRead(bool readable)
    {
        return DelegateSupport.ConvertDelegate<Func<Item, bool>>((Item item) =>
        {
            if (item.Template == null || !item.IsReadable)
            {
                return false;
            }

            var heroReadables = Hero.Current.Element<HeroReadables>();
            if (heroReadables == null)
            {
                Plugin.Log.LogError("HeroReadables is null, can't check if item is readable");
                return false;
            }

            return readable
                ? heroReadables.WasTemplateRead(item.Template)
                : !heroReadables.WasTemplateRead(item.Template);
        });
    }
}
