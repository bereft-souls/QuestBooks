using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    /// <summary>
    /// A simple canvas element that contains quest information.
    /// </summary>
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
