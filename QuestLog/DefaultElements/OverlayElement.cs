using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultLogStyles;
using QuestBooks.Systems;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultElements
{
    /// <summary>
    /// A simple canvas element that overlays a texture above your canvas and does NOT shift with moving canvases.
    /// </summary>
    [ElementTooltip("OverlayElement")]
    public class OverlayElement : ChapterElement
    {
        public override float DrawPriority => 0.9f;

        // Used when the texture is not found or has not been assigned yet.
        private const string DefaultTexture = "QuestBooks/Assets/Textures/Elements/QuestionMark";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/Elements/QuestionMarkOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);
        private static readonly Asset<Texture2D> DefaultOutlineAsset = ModContent.Request<Texture2D>(DefaultOutline);

        // Concise autoproperties coming in C# 13....
        [JsonProperty]
        private string _texturePath = DefaultTexture;

        [JsonIgnore]
        private Asset<Texture2D> _texture = null;

        [JsonIgnore]
        [UseConverter(typeof(TextureChecker))]
        [ElementTooltip("DisplayTexture")]
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

        [ElementTooltip("DisplayScale")]
        public float Scale { get; set; } = 1f;

        [UseConverter(typeof(AngleConverter))]
        [ElementTooltip("DisplayRotation")]
        public float Rotation { get; set; } = 0f;

        public override bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, ref string mouseTooltip)
        {
            _texture ??= ModContent.Request<Texture2D>(_texturePath);
            return QuestManager.ActiveStyle.UseDesigner && CenteredRectangle(CanvasPosition, _texture.Size()).Contains((mousePosition - canvasViewOffset).ToPoint());
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            _texture ??= ModContent.Request<Texture2D>(_texturePath);
            Texture2D texture = _texture.Value;
            Vector2 drawPos = CanvasPosition;

            if (texture == DefaultAsset.Value)
            {
                Texture2D outline = DefaultOutlineAsset.Value;

                if (selected)
                    spriteBatch.Draw(outline, drawPos, null, Color.Yellow, Rotation, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);

                else if (hovered)
                    spriteBatch.Draw(outline, drawPos, null, Color.White, Rotation, outline.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, drawPos, null, Color.White, Rotation, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            CanvasPosition = mousePosition - canvasViewOffset;
            return true;
        }

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            if (_texture is null || _texture.Value is null)
            {
                base.DrawPlacementPreview(spriteBatch, mousePosition, canvasViewOffset);
                return;
            }

            Texture2D texture = _texture.Value;
            spriteBatch.Draw(texture, mousePosition - canvasViewOffset, null, Color.White with { A = 180 }, 0f, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
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
