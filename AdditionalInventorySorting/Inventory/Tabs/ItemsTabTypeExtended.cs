using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Scenes.SceneConstructors;
using Awaken.TG.Main.Templates;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace AdditionalInventorySorting.Inventory.Tabs;

public enum PotionType
{
    Other,
    Health,
    Mana,
    Stamina
}

public static class ItemsTabTypeExtended
{
    public static readonly ItemsTabType ReadableUnread = New(nameof(ReadableUnread), CheckRead(false),
        Plugin.LanguageConfig.ReadableSubTypeUnreadDisplayName.Value);

    public static readonly ItemsTabType ReadableRead = New(nameof(ReadableRead), CheckRead(true),
        Plugin.LanguageConfig.ReadableSubTypeReadDisplayName.Value);


    public static readonly ItemsTabType PotionHealth = New(nameof(PotionHealth), CheckPotion(PotionType.Health),
        Plugin.LanguageConfig.PotionSubTypeHealthDisplayName.Value);

    public static readonly ItemsTabType PotionMana = New(nameof(PotionMana), CheckPotion(PotionType.Mana),
        Plugin.LanguageConfig.PotionSubTypeManaDisplayName.Value);

    public static readonly ItemsTabType PotionStamina = New(nameof(PotionStamina), CheckPotion(PotionType.Stamina),
        Plugin.LanguageConfig.PotionSubTypeStaminaDisplayName.Value);

    private static ItemsTabType New(string name, Func<Item, bool> filter, string displayName) =>
        new(name, filter) { _title = LocStringUtils.New(nameof(ItemsTabTypeExtended), name, displayName) };


    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember(ReadableUnread) &&
               RichEnumPatcher.AddOrUpdateMember(ReadableRead) &&
               RichEnumPatcher.AddOrUpdateMember(PotionHealth) &&
               RichEnumPatcher.AddOrUpdateMember(PotionMana) &&
               RichEnumPatcher.AddOrUpdateMember(PotionStamina);
    }

    private static Func<Item, bool> CheckRead(bool read)
    {
        return DelegateSupport.ConvertDelegate<Func<Item, bool>>((Item item) =>
        {
            if (item.Template == null || !item.IsReadable)
            {
                return false;
            }

            HeroReadables heroReadables = Hero.Current.Element<HeroReadables>();
            if (heroReadables == null)
            {
                Plugin.Log.LogError("HeroReadables is null, can't check if item is readable");
                return false;
            }

            return read
                ? heroReadables.WasTemplateRead(item.Template)
                : !heroReadables.WasTemplateRead(item.Template);
        });
    }

    private static Func<Item, bool> CheckPotion(PotionType potionType)
    {
        return DelegateSupport.ConvertDelegate<Func<Item, bool>>((Item item) => GetPotionType(item) == potionType);
    }

    private static PotionType GetPotionType(Item item)
    {
        TemplateService templateService = CommonReferences.Get.TemplateService;
        ITemplate itemTemplate = item.Template.Cast<ITemplate>();

        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableHealth))
            return PotionType.Health;
        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableMana))
            return PotionType.Mana;
        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableStamina))
            return PotionType.Stamina;
        return PotionType.Other;
    }
}
