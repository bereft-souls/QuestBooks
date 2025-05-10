using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    /// <summary>
    /// A basic <see cref="QuestBook"/> implementation, containing a set of <see cref="QuestLine"/>s.
    /// </summary>
    public class BasicQuestBook : QuestBook
    {
        [JsonIgnore]
        public override IEnumerable<QuestLine> Chapters => QuestLines;

        public List<QuestLine> QuestLines = [];

        public override void Update()
        {
            foreach (QuestLine questLine in QuestLines)
                questLine.Update();
        }
    }
}
