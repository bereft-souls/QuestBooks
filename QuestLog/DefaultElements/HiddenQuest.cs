using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static QuestBooks.QuestLog.DefaultElements.QuestDisplay;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("HiddenQuest")]
    public class HiddenQuest : ChapterElement, IConnectable
    {
        [UseConverter(typeof(QuestChecker))]
        [ElementTooltip("QuestKey")]
        public string QuestKey { get; set; } = new Placeholder().Key;

        // Used when the texture is not found or has not been assigned yet.
        private const string DefaultTexture = "QuestBooks/Assets/Textures/Quests/Medium";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/Quests/MediumOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);
        private static readonly Asset<Texture2D> DefaultOutlineAsset = ModContent.Request<Texture2D>(DefaultOutline);

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - QuestManager.ActiveStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public override bool VisibleOnCanvas() => QuestManager.ActiveStyle.UseDesigner;

        public bool CompleteConnection(IConnectable source) => false;

        public bool ConnectionVisible(IConnectable destination) => false;

        public bool ConnectionActive(IConnectable destination) => QuestManager.GetQuest(QuestKey).Completed;

        // mousePosition is already in logical canvas coordinates (zoom factored out)
        public override bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, float zoom, ref string mouseTooltip) => CenteredRectangle(CanvasPosition, DefaultAsset.Size()).Contains(mousePosition.ToPoint());

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool selected, bool hovered)
        {
            if (selected)
                DrawTexture(spriteBatch, DefaultOutlineAsset.Value, canvasViewOffset, zoom, Color.Yellow);

            else if (hovered)
                DrawTexture(spriteBatch, DefaultOutlineAsset.Value, canvasViewOffset, zoom, Color.LightGray);

            DrawTexture(spriteBatch, DefaultAsset.Value, canvasViewOffset, zoom, Color.White);
        }

        protected void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 canvasOffset, float zoom, Color color)
        {
            Vector2 drawPos = (CanvasPosition - canvasOffset) * zoom;
            spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.MediumQuest, iconArea);

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset, float zoom)
        {
            Texture2D texture = DefaultAsset.Value;
            Vector2 drawPos = (mousePosition - canvasViewOffset) * zoom;
            spriteBatch.Draw(texture, drawPos, null, Color.White with { A = 220 }, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override void OnDelete() => this.DeleteConnections();
    }
}
