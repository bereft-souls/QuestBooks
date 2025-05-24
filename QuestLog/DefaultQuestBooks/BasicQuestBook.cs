using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultQuestLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    /// <summary>
    /// A basic <see cref="QuestBook"/> implementation, containing a set of <see cref="QuestLine"/>s.
    /// </summary>
    public class BasicQuestBook : QuestBook
    {
        [JsonIgnore]
        public override IEnumerable<QuestLine> Chapters => QuestLines;

        [JsonIgnore]
        public virtual string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        public List<BasicQuestLine> QuestLines = [];

        public override void Update()
        {
            foreach (BasicQuestLine questLine in QuestLines)
                questLine.Update();
        }
    }
}
