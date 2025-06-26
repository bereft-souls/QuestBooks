using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;
using Terraria;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    /// <summary>
    /// A simple canvas element that contains quest information.
    /// </summary>
    public class BasicQuestElement : QuestLineElement
    {
        [JsonIgnore]
        public Quest Quest => QuestManager.GetQuest(QuestKey);

        public Vector2 CanvasPosition { get; set; }

        public string QuestKey { get; set; } = "QuestKey";

        public float Scale { get; set; } = 1f;

        public override bool IsHovered(Vector2 mousePosition)
        {
            return CenteredRectangle(CanvasPosition, QuestAssets.MissingIcon.Value.Size()).Contains(mousePosition.ToPoint());
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool hovered)
        {
            Texture2D texture = QuestAssets.MissingIcon;

            if (hovered)
                spriteBatch.Draw(texture, CanvasPosition, null, Color.Yellow, 0f, texture.Size() * 0.5f, Scale * 1.1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(texture, CanvasPosition, null, Color.White, 0f, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(QuestLine chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            chapter.Elements.Add(this);
            return true;
        }
    }
}
