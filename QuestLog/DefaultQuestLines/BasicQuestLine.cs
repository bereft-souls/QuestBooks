using QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements;
using QuestBooks.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultQuestLines
{
    public class BasicQuestLine : QuestLine
    {
        public List<QuestLineElement> Elements = [];

        public IEnumerable<Quest> QuestList => Elements.Where(e => e is QuestElement).Cast<QuestElement>().Select(e => e.Quest);

        public float Progress => (float)QuestList.Count(q => q.Completed) / Math.Max(1, QuestList.Count());

        public bool Complete => QuestList.All(q => q.Completed);

        public override void Update()
        {
            foreach (QuestLineElement questElement in Elements)
                questElement.Update();
        }

        public override void DrawChapter()
        {
            throw new NotImplementedException();
        }

        public override void DrawContents()
        {
            throw new NotImplementedException();
        }
    }
}
