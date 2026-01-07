using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Systems;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("QuestJump")]
    internal class QuestJumpElement : ChapterElement, IConnectable
    {
        [JsonIgnore]
        public int IncomingFeeds => Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(this));

        [ElementTooltip("DisplayPrerequisites")]
        public virtual int DisplayFeeds { get; set; } = 0;

        [ElementTooltip("UnlockPrerequisistes")]
        public virtual int UnlockFeeds { get; set; } = 0;

        private const string DefaultTexture = "QuestBooks/Assets/Textures/Quests/Diamond";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/Quests/DiamondOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);

        [JsonProperty] private string _outlineTexturePath = DefaultOutline;
        [JsonProperty] private string _lockedTexturePath = "";
        [JsonProperty] private string _unlockedTexturePath = DefaultTexture;

        [JsonIgnore] protected Asset<Texture2D> _outlineTexture = null;
        [JsonIgnore] protected Asset<Texture2D> _lockedTexture = null;
        [JsonIgnore] protected Asset<Texture2D> _unlockedTexture = null;

        [JsonProperty]
        [UseConverter(typeof(QuestBookChecker))]
        [ElementTooltip("JumpBook")]
        public virtual QuestBook JumpBook { get; set; } = QuestManager.QuestBooks.FirstOrDefault(defaultValue: null);

        [JsonProperty]
        [UseConverter(typeof(QuestChapterChecker))]
        [ElementTooltip("JumpChapter")]
        public virtual BookChapter JumpChapter { get; set; } = QuestManager.QuestBooks.FirstOrDefault(defaultValue: null)?.Chapters.FirstOrDefault(defaultValue: null) ?? null;

        [JsonProperty]
        [UseConverter(typeof(Vector2Converter))]
        [ElementTooltip("JumpOffset")]
        public virtual Vector2 JumpOffset { get; set; } = Vector2.Zero;

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("UnlockedTexture")]
        public virtual string Texture
        {
            get => _unlockedTexturePath;
            set { _unlockedTexturePath = value; _unlockedTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("OutlineTexture")]
        public virtual string OutlineTexture
        {
            get => _outlineTexturePath;
            set { _outlineTexturePath = value; _outlineTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(QuestDisplay.TextureCheckerEmptyAllowed))]
        [ElementTooltip("LockedTexture")]
        public virtual string LockedTexture
        {
            get => _lockedTexturePath;
            set { _lockedTexturePath = value; _lockedTexture = null; }
        }

        [ElementTooltip("TooltipLocalization")]
        public string TooltipLocalization { get; set; } = "Mods.QuestBooks.Tooltips.Elements.QuestJumpHover";

        [JsonIgnore]
        [HideInDesigner]
        public string Tooltip { get => string.IsNullOrWhiteSpace(TooltipLocalization) ? null : Language.GetOrRegister(TooltipLocalization).Value; }

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - QuestManager.ActiveStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public override bool VisibleOnCanvas() => IncomingFeeds >= DisplayFeeds || QuestManager.ActiveStyle.UseDesigner;
        public bool Unlocked() => IncomingFeeds >= UnlockFeeds;

        public bool CompleteConnection(IConnectable source) => VisibleOnCanvas();
        public bool ConnectionVisible(IConnectable destination) => VisibleOnCanvas();
        public bool ConnectionActive(IConnectable destination) => true;

        public override bool IsHovered(Vector2 mousePosition, ref string mouseTooltip)
        {
            _unlockedTexture ??= ModContent.Request<Texture2D>(_unlockedTexturePath);
            bool hovered = CenteredRectangle(CanvasPosition, _unlockedTexture.Size()).Contains(mousePosition.ToPoint());

            string tooltip = Tooltip;
            if (hovered && tooltip is not null)
                mouseTooltip = tooltip;

            return hovered;
        }

        public override void OnSelect()
        {
            QuestManager.ActiveStyle.SelectBook(JumpBook);
            QuestManager.ActiveStyle.SelectChapter(JumpChapter);

            if (JumpChapter.EnableShifting)
                QuestManager.ActiveStyle.QuestAreaOffset = JumpOffset;
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            if (QuestManager.ActiveStyle.UseDesigner)
            {
                int cycle = (int)(Main.timeForVisualEffects % 120 / 60);
                switch (cycle)
                {
                    case 0:
                        DrawLocked(spriteBatch);
                        return;

                    default:
                        DrawUnlocked(spriteBatch, hovered, selected);
                        return;
                }
            }

            if (!Unlocked())
                DrawLocked(spriteBatch);

            else
                DrawUnlocked(spriteBatch, hovered, selected);
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

            _unlockedTexture ??= ModContent.Request<Texture2D>(_unlockedTexturePath);
            DrawOutline(spriteBatch, Color.Black);
            DrawTexture(spriteBatch, _unlockedTexture.Value, Color.Black);
        }

        protected virtual void DrawUnlocked(SpriteBatch spriteBatch, bool hovered, bool selected)
        {
            if (hovered )
                DrawOutline(spriteBatch, Color.LightGray);

            else
                DrawOutline(spriteBatch, new(108, 118, 199, 255));

            _unlockedTexture ??= ModContent.Request<Texture2D>(_unlockedTexturePath);
            DrawTexture(spriteBatch, _unlockedTexture.Value, Color.White);
        }

        protected void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            Vector2 drawPos = CanvasPosition - QuestManager.ActiveStyle.QuestAreaOffset;
            spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.QuestJump, iconArea);

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            Texture2D texture = DefaultAsset.Value;
            spriteBatch.Draw(texture, mousePosition - canvasViewOffset, null, Color.White with { A = 220 }, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override void OnDelete()
        {
            // Clone the collection to allow modified enumeration
            foreach (var connection in Connections.ToArray())
            {
                QuestManager.ActiveStyle.SelectedChapter.Elements.Remove(connection);
                connection.OnDelete();
            }
        }

        public class QuestBookChecker : IMemberConverter<QuestBook>
        {
            public ChapterElement CallingElement { set => JumpElement = value as QuestJumpElement; }
            public QuestJumpElement JumpElement;

            public string Convert(QuestBook input) => input is null ? "0" : QuestManager.QuestBooks.IndexOf(input).ToString();
            public bool TryParse(string input, out QuestBook result)
            {
                if (!int.TryParse(input, out int index))
                {
                    result = null;
                    return false;
                }

                if (index < 0 || index >= (QuestManager.QuestBooks?.Count ?? 0))
                {
                    if (index == 0)
                    {
                        result = null;
                        return true;
                    }

                    result = null;
                    return false;
                }

                result = QuestManager.QuestBooks[index];

                int chapterIndex = (JumpElement.JumpBook is null || JumpElement.JumpChapter is null) ? 0 : JumpElement.JumpBook.Chapters.IndexOf(JumpElement.JumpChapter);
                if (chapterIndex >= result.Chapters.Count)
                    chapterIndex = 0;

                JumpElement.JumpChapter = result.Chapters[chapterIndex];
                return true;
            }
        }

        public class QuestChapterChecker : IMemberConverter<BookChapter>
        {
            public ChapterElement CallingElement { set => JumpElement = value as QuestJumpElement; }
            public QuestJumpElement JumpElement;

            public string Convert(BookChapter input) => input is null ? "0" : JumpElement.JumpBook.Chapters.IndexOf(input).ToString();
            public bool TryParse(string input, out BookChapter result)
            {
                if (!int.TryParse(input, out int index))
                {
                    result = null;
                    return false;
                }

                var chapters = JumpElement.JumpBook?.Chapters ?? [];

                if (index < 0 || index >= chapters.Count)
                {
                    if (index == 0)
                    {
                        result = null;
                        return true;
                    }

                    result = null;
                    return false;
                }

                result = chapters[index];
                return true;
            }
        }
    }
}
