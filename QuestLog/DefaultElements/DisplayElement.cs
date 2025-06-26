using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
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
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);

        // Concise autoproperties coming in C# 13....
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
        protected Asset<Texture2D> DisplayAsset = DefaultAsset;

        /// <summary>
        /// The position within the canvas to display this element.
        /// </summary>
        public Vector2 CanvasPosition { get; set; }

        public float Scale { get; set; } = 1f;

        public override bool IsHovered(Vector2 mousePosition)
        {
            return CenteredRectangle(CanvasPosition, DisplayAsset.Value.Size()).Contains(mousePosition.ToPoint());
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool hovered)
        {
            Texture2D texture = DisplayAsset.Value;

            if (hovered)
                spriteBatch.Draw(texture, CanvasPosition, null, Color.Yellow, 0f, texture.Size() * 0.5f, Scale * 1.1f, SpriteEffects.None, 0f);

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
