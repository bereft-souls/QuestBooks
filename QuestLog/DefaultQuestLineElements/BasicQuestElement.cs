using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    public class BasicQuestElement : QuestElement
    {
        public string QuestName { get; set; }

        [JsonIgnore]
        public override Quest Quest => QuestManager.GetQuest(QuestName);

        public override void DrawToCanvas()
        {
            throw new NotImplementedException();
        }
    }
}
