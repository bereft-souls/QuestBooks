using System;

namespace QuestBooks.QuestLog
{
    /// <summary>
    /// Marks your class/member with a localization key to be displayed in the mouse tooltip whenever the element is hovered in the designer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class TooltipAttribute(string localizationKey) : Attribute
    {
        public virtual string LocalizationKey { get; init; } = localizationKey;
    }
}
