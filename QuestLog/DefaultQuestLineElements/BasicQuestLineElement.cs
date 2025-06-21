using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    public abstract class BasicQuestLineElement() : QuestLineElement
    {
        public Vector2 CanvasPosition { get; set; }

        public override void DrawToCanvas(Vector2 offset)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A simple canvas element that contains quest information.
    /// </summary>
    public class BasicQuestElement : BasicQuestLineElement
    {
        [JsonIgnore]
        public Quest Quest => QuestManager.GetQuest(QuestName);

        public string QuestName { get; set; }
    }
}
