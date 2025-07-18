using System;
using AdditionalInventorySorting.Utils;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Tabs;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Scenes.SceneConstructors;
using Awaken.TG.Main.Templates;

namespace AdditionalInventorySorting.Inventory.Tabs;

public enum PotionType
{
    Other,
    Health,
    Mana,
    Stamina
}

public class ItemsTabTypeExtended : ItemsTabType
{
    public static readonly ItemsTabTypeExtended ReadableUnread = new(nameof(ReadableUnread), CheckRead(false),
        Plugin.LanguageConfig.ReadableSubTypeUnreadDisplayName.Value);

    public static readonly ItemsTabTypeExtended ReadableRead = new(nameof(ReadableRead), CheckRead(true),
        Plugin.LanguageConfig.ReadableSubTypeReadDisplayName.Value);

    public static readonly ItemsTabTypeExtended PotionHealth = new(nameof(PotionHealth), CheckPotion(PotionType.Health),
        Plugin.LanguageConfig.PotionSubTypeHealthDisplayName.Value);

    public static readonly ItemsTabTypeExtended PotionMana = new(nameof(PotionMana), CheckPotion(PotionType.Mana),
        Plugin.LanguageConfig.PotionSubTypeManaDisplayName.Value);

    public static readonly ItemsTabTypeExtended PotionStamina = new(nameof(PotionStamina),
        CheckPotion(PotionType.Stamina),
        Plugin.LanguageConfig.PotionSubTypeStaminaDisplayName.Value);

    private ItemsTabTypeExtended(string enumName, Func<Item, bool> filter, string title,
        ItemsTabType[] subTabs = null, Func<Item, bool> gridException = null) : base(enumName, filter, "null", subTabs,
        gridException)
    {
        LocStringUtils.Initialize(_title, nameof(ItemsTabTypeExtended), enumName, title);
    }

    public static bool InjectMembers()
    {
        return RichEnumPatcher.AddOrUpdateMember<ItemsTabType>(ReadableUnread) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsTabType>(ReadableRead) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsTabType>(PotionHealth) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsTabType>(PotionMana) &&
               RichEnumPatcher.AddOrUpdateMember<ItemsTabType>(PotionStamina);
    }

    private static Func<Item, bool> CheckRead(bool read)
    {
        return item =>
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
        };
    }

    private static Func<Item, bool> CheckPotion(PotionType potionType)
    {
        return item => GetPotionType(item) == potionType;
    }

    private static PotionType GetPotionType(Item item)
    {
        TemplateService templateService = CommonReferences.Get.TemplateService;
        ITemplate itemTemplate = item.Template;

        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableHealth))
            return PotionType.Health;
        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableMana))
            return PotionType.Mana;
        if (itemTemplate.InheritsFrom(templateService.AbstractConsumableStamina))
            return PotionType.Stamina;
        return PotionType.Other;
    }
}
