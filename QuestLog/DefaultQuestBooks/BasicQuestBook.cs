using System.Collections.Generic;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    /// <summary>
    /// A simple quest book that provides a localization key for a display name.<br/>
    /// This class is abstract and should be inherited.
    /// </summary>
    public abstract class BasicQuestBook : QuestBook
    {
        public override List<BookChapter> Chapters { get; set; } = [];

        public override string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        public override void CloneTo(QuestBook newInstance)
        {
            if (newInstance is BasicQuestBook book)
                book.NameKey = NameKey;

            base.CloneTo(newInstance);
        }

        internal sealed class BookTooltip(string localizationKey) : TooltipAttribute($"Mods.QuestBooks.Tooltips.Library.{localizationKey}");
    }
}
