using System.Collections.Generic;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
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
