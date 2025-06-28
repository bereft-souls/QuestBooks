using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    public abstract class BasicQuestBook : QuestBook
    {
        public override List<BookChapter> Chapters { get; set; } = [];

        public override string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        public override void Update()
        {
            foreach (BookChapter questLine in Chapters)
                questLine.Update();
        }

        public override void CloneTo(QuestBook newInstance)
        {
            if (newInstance is BasicQuestBook book)
                book.NameKey = NameKey;

            base.CloneTo(newInstance);
        }
    }
}
