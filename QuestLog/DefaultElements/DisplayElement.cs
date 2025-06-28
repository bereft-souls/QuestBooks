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
        [JsonProperty]
        private string _texturePath = DefaultTexture;

        [JsonIgnore]
        private Asset<Texture2D> _texture = null;

        [JsonIgnore]
        [UseConverter(typeof(TextureChecker))]
        public string TexturePath
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _texturePath;
            set { _texturePath = value; _texture = null; }
        }

        /// <summary>
        /// The position within the canvas to display this element.
        /// </summary>
        public Vector2 CanvasPosition { get; set; }

        public float Scale { get; set; } = 1f;

        public float Rotation { get; set; } = 0f;

        public override bool IsHovered(Vector2 mousePosition)
        {
            _texture ??= ModContent.Request<Texture2D>(_texturePath);
            return BasicQuestLogStyle.UseDesigner && CenteredRectangle(CanvasPosition, _texture.Size()).Contains(mousePosition.ToPoint());
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            _texture ??= ModContent.Request<Texture2D>(_texturePath);
            Texture2D texture = _texture.Value;

            if (texture == DefaultAsset.Value)
            {
                Texture2D outline = DefaultOutlineAsset.Value;

                if (selected)
                    spriteBatch.Draw(outline, CanvasPosition, null, Color.Yellow, Rotation, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);

                else if (hovered)
                    spriteBatch.Draw(outline, CanvasPosition, null, Color.White, Rotation, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, CanvasPosition, null, Color.White, Rotation, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            chapter.Elements.Add(this);
            return true;
        }

        public class TextureChecker : IMemberConverter<string>
        {
            public string Convert(string input) => input;
            public bool TryParse(string input, out string result)
            {
                result = input;
                return ModContent.FileExists($"{input}.rawimg");
            }
        }
    }
}
