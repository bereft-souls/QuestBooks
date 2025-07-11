using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using QuestBooks.Quests;
using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultElements
{
    public class QuestDisplay : QuestElement, IConnectable
    {
        [UseConverter(typeof(QuestChecker))]
        public string QuestKey { get; set; } = new Placeholder().Key;

        public override Quest Quest => QuestManager.GetQuest(QuestKey);

        [JsonIgnore]
        public int IncomingFeeds => Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(this));

        [ElementTooltip("DisplayPrerequisites")]
        public int DisplayPrerequisites { get; set; } = 0;

        [ElementTooltip("UnlockPrerequisites")]
        public int UnlockPrerequisites { get; set; } = 0;

        // Used when the texture is not found or has not been assigned yet.
        private const string DefaultTexture = "QuestBooks/Assets/Textures/Quests/Medium";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/Quests/MediumOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);

        // Concise autoproperties coming in C# 13....
        [JsonProperty] private string _outlineTexturePath = DefaultOutline;
        [JsonProperty] private string _lockedTexturePath = "";
        [JsonProperty] private string _incompleteTexturePath = "";
        [JsonProperty] private string _completedTexturePath = DefaultTexture;

        [JsonIgnore] private Asset<Texture2D> _outlineTexture = null;
        [JsonIgnore] private Asset<Texture2D> _lockedTexture = null;
        [JsonIgnore] private Asset<Texture2D> _incompleteTexture = null;
        [JsonIgnore] private Asset<Texture2D> _completedTexture = null;

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("CompletedTexture")]
        public string Texture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _completedTexturePath;
            set { _completedTexturePath = value; _completedTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("OutlineTexture")]
        public string OutlineTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _outlineTexturePath;
            set { _outlineTexturePath = value; _outlineTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(TextureCheckerEmptyAllowed))]
        [ElementTooltip("LockedTexture")]
        public string LockedTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _lockedTexturePath;
            set { _lockedTexturePath = value; _lockedTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(TextureCheckerEmptyAllowed))]
        [ElementTooltip("IncompleteTexture")]
        public string IncompleteTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _incompleteTexturePath;
            set { _incompleteTexturePath = value; _incompleteTexture = null; }
        }

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - BasicQuestLogStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public override bool VisibleOnCanvas() => IncomingFeeds >= DisplayPrerequisites || BasicQuestLogStyle.UseDesigner;
        public bool Unlocked() => IncomingFeeds >= UnlockPrerequisites;
        public bool Completed() => Quest.Completed;

        public bool CompleteConnection(IConnectable source) => VisibleOnCanvas();

        public bool ConnectionVisible(IConnectable destination) => VisibleOnCanvas();

        public bool ConnectionActive(IConnectable destination) => Quest.Completed;

        public override bool IsHovered(Vector2 mousePosition, ref string mouseTooltip)
        {
            _completedTexture ??= ModContent.Request<Texture2D>(_completedTexturePath);
            bool hovered = CenteredRectangle(CanvasPosition, _completedTexture.Size()).Contains(mousePosition.ToPoint());

            if (hovered && Quest is ProgressionQuest quest && !string.IsNullOrWhiteSpace(quest.HoverTooltip))
                mouseTooltip = Language.GetTextValue(quest.HoverTooltip);

            return hovered;
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            if (selected)
                DrawOutline(spriteBatch, Color.Yellow);

            else if (hovered)
                DrawOutline(spriteBatch, Color.LightGray);

            if (BasicQuestLogStyle.UseDesigner)
            {
                int cycle = (int)(Main.timeForVisualEffects % 180 / 60);
                switch (cycle)
                {
                    case 0:
                        DrawLocked(spriteBatch);
                        return;

                    case 1:
                        DrawIncomplete(spriteBatch);
                        return;

                    default:
                        DrawCompleted(spriteBatch);
                        return;
                }
            }

            if (!Unlocked())
                DrawLocked(spriteBatch);

            else if (!Completed())
                DrawIncomplete(spriteBatch);

            else
                DrawCompleted(spriteBatch);
        }

        protected virtual void DrawOutline(SpriteBatch spriteBatch, Color color)
        {
            _outlineTexture ??= ModContent.Request<Texture2D>(_outlineTexturePath);
            DrawTexture(spriteBatch, _outlineTexture.Value, color);
        }

        protected virtual void DrawLocked(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrWhiteSpace(_lockedTexturePath))
            {
                _lockedTexture ??= ModContent.Request<Texture2D>(_lockedTexturePath);
                DrawTexture(spriteBatch, _lockedTexture.Value, Color.White);
                return;
            }

            _completedTexture ??= ModContent.Request<Texture2D>(_completedTexturePath);
            DrawTexture(spriteBatch, _completedTexture.Value, Color.Black);
        }

        protected virtual void DrawIncomplete(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrWhiteSpace(_incompleteTexturePath))
            {
                _incompleteTexture ??= ModContent.Request<Texture2D>(_incompleteTexturePath);
                DrawTexture(spriteBatch, _incompleteTexture.Value, Color.White);
                return;
            }

            _completedTexture ??= ModContent.Request<Texture2D>(_completedTexturePath);
            Effect grayscale = QuestAssets.Grayscale;

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, grayscale, matrix);
            DrawTexture(spriteBatch, _completedTexture.Value, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        protected virtual void DrawCompleted(SpriteBatch spriteBatch)
        {
            _completedTexture ??= ModContent.Request<Texture2D>(_completedTexturePath);
            DrawTexture(spriteBatch, _completedTexture.Value, Color.White);
        }

        protected void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Color color) =>
            spriteBatch.Draw(texture, CanvasPosition - BasicQuestLogStyle.QuestAreaOffset, null, color, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.MediumQuest, iconArea);

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            Texture2D texture = DefaultAsset.Value;
            spriteBatch.Draw(texture, mousePosition - canvasViewOffset, null, Color.White with { A = 180 }, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override bool HasInfoPage => Quest is ProgressionQuest quest && (!string.IsNullOrWhiteSpace(quest.PageTitle) || !string.IsNullOrWhiteSpace(quest.PageContents));
        public override void DrawInfoPage(SpriteBatch spriteBatch)
        {
            base.DrawInfoPage(spriteBatch);
        }

        public class QuestChecker : IMemberConverter<string>
        {
            public string Convert(string input) => input;
            public bool TryParse(string input, out string result)
            {
                result = input;
                return QuestManager.TryGetQuest(input, out _);
            }
        }

        public class TextureCheckerEmptyAllowed : IMemberConverter<string>
        {
            public string Convert(string input) => input;
            public bool TryParse(string input, out string result)
            {
                result = input;
                return string.IsNullOrWhiteSpace(input) || ModContent.FileExists($"{input}.rawimg");
            }
        }

    }
}
