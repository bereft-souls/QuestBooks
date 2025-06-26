using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    /// <summary>
    /// A simple canvas element that contains quest information.
    /// </summary>
    public class BasicQuestElement : QuestLineElement
    {
        [JsonIgnore]
        public Quest Quest => QuestManager.GetQuest(QuestName);

        public string QuestName { get; set; }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset)
        {
            throw new NotImplementedException();
        }
    }
}
