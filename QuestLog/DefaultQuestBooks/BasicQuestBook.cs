using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    public class BasicQuestBook : QuestBook
    {
        public List<QuestLine> QuestLines = [];

        public override void Update()
        {
            foreach (QuestLine questLine in QuestLines)
                questLine.Update();
        }

        public override void DrawBook()
        {
            throw new NotImplementedException();
        }

        public override void DrawChapters()
        {
            throw new NotImplementedException();
        }
    }
}
