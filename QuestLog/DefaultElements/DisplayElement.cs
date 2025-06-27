using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultElements
{
    /// <summary>
    /// A simple canvas element that contains a texture to display and scale.
    /// </summary>
    public class DisplayElement : ChapterElement
    {
        // Used when the texture is not found or has not been assigned yet.
        private const string DefaultTexture = "QuestBooks/Assets/Textures/QuestionMark";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/QuestionMarkOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);
        private static readonly Asset<Texture2D> DefaultOutlineAsset = ModContent.Request<Texture2D>(DefaultOutline);

        // Concise autoproperties coming in C# 13....
        [HideInDesigner]
        private string _texture = DefaultTexture;

        [JsonIgnore]
        public string Texture
        {
            get => _texture;
            set
            {
                _texture = value;

                // Fetch the texture when the string is changed
                if (ModContent.RequestIfExists<Texture2D>(_texture, out var asset))
                    DisplayAsset = asset;

                else
                    DisplayAsset = DefaultAsset;
            }
        }

        [JsonIgnore]
        [HideInDesigner]
        protected Asset<Texture2D> DisplayAsset = DefaultAsset;

        /// <summary>
        /// The position within the canvas to display this element.
        /// </summary>
        [HideInDesigner]
        public Vector2 CanvasPosition { get; set; }

        public float Scale { get; set; } = 1f;

        public override bool IsHovered(Vector2 mousePosition)
        {
            return BasicQuestLogStyle.UseDesigner && CenteredRectangle(CanvasPosition, DisplayAsset.Value.Size()).Contains(mousePosition.ToPoint());
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            Texture2D texture = DisplayAsset.Value;

            if (texture == DefaultAsset.Value)
            {
                Texture2D outline = DefaultOutlineAsset.Value;

                if (selected)
                    spriteBatch.Draw(outline, CanvasPosition, null, Color.Yellow, 0f, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);

                else if (hovered)
                    spriteBatch.Draw(outline, CanvasPosition, null, Color.White, 0f, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, CanvasPosition, null, Color.White, 0f, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            chapter.Elements.Add(this);
            return true;
        }
    }
}
