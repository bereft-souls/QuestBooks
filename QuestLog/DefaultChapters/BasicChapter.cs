using System.Collections.Generic;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultChapters
{
    public abstract class BasicChapter : BookChapter
    {
        public override List<ChapterElement> Elements { get; set; } = [];

        public override string DisplayName => Language.GetOrRegister(NameKey).Value;

        /// <summary>
        /// The localization key used to display this line's title.
        /// </summary>
        public string NameKey;

        public override void CloneTo(BookChapter newInstance)
        {
            if (newInstance is BasicChapter chapter)
                chapter.NameKey = NameKey;

            base.CloneTo(newInstance);
        }

        internal sealed class ChapterTooltip(string localizationKey) : TooltipAttribute($"Mods.QuestBooks.Tooltips.Library.{localizationKey}");
    }
}
