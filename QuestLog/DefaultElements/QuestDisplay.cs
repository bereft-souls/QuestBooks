﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using QuestBooks.Quests;
using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("QuestDisplay")]
    public class QuestDisplay : QuestElement, IConnectable
    {
        [UseConverter(typeof(QuestChecker))]
        [ElementTooltip("QuestKey")]
        public virtual string QuestKey { get; set; } = new Placeholder().Key;

        // Already has JsonIgnore
        public override Quest Quest => QuestManager.GetQuest(QuestKey);

        [JsonIgnore]
        public int IncomingFeeds => Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(this));

        [ElementTooltip("DisplayPrerequisites")]
        public virtual int DisplayFeeds { get; set; } = 0;

        [ElementTooltip("UnlockPrerequisites")]
        public virtual int UnlockFeeds { get; set; } = 0;

        // Used when the texture is not found or has not been assigned yet.
        private const string DefaultTexture = "QuestBooks/Assets/Textures/Quests/Medium";
        private const string DefaultOutline = "QuestBooks/Assets/Textures/Quests/MediumOutline";
        private static readonly Asset<Texture2D> DefaultAsset = ModContent.Request<Texture2D>(DefaultTexture);

        // Concise autoproperties coming in C# 13....
        [JsonProperty] private string _outlineTexturePath = DefaultOutline;
        [JsonProperty] private string _lockedTexturePath = "";
        [JsonProperty] private string _incompleteTexturePath = "";
        [JsonProperty] private string _completedTexturePath = DefaultTexture;

        [JsonIgnore] protected Asset<Texture2D> _outlineTexture = null;
        [JsonIgnore] protected Asset<Texture2D> _lockedTexture = null;
        [JsonIgnore] protected Asset<Texture2D> _incompleteTexture = null;
        [JsonIgnore] protected Asset<Texture2D> _completedTexture = null;

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("CompletedTexture")]
        public virtual string Texture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _completedTexturePath;
            set { _completedTexturePath = value; _completedTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(DisplayElement.TextureChecker))]
        [ElementTooltip("OutlineTexture")]
        public virtual string OutlineTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _outlineTexturePath;
            set { _outlineTexturePath = value; _outlineTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(TextureCheckerEmptyAllowed))]
        [ElementTooltip("LockedTexture")]
        public virtual string LockedTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _lockedTexturePath;
            set { _lockedTexturePath = value; _lockedTexture = null; }
        }

        [JsonIgnore]
        [UseConverter(typeof(TextureCheckerEmptyAllowed))]
        [ElementTooltip("IncompleteTexture")]
        public virtual string IncompleteTexture
        {
            // Because of our custom converter, this will only ever be
            // set if the texture path is valid
            get => _incompleteTexturePath;
            set { _incompleteTexturePath = value; _incompleteTexture = null; }
        }

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - BasicQuestLogStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public override bool VisibleOnCanvas() => IncomingFeeds >= DisplayFeeds || BasicQuestLogStyle.UseDesigner;
        public bool Unlocked() => IncomingFeeds >= UnlockFeeds;
        public bool Completed() => Quest.Completed;

        public bool CompleteConnection(IConnectable source) => VisibleOnCanvas();

        public bool ConnectionVisible(IConnectable destination) => VisibleOnCanvas();

        public bool ConnectionActive(IConnectable destination) => Quest.Completed;

        public override bool IsHovered(Vector2 mousePosition, ref string mouseTooltip)
        {
            _completedTexture ??= ModContent.Request<Texture2D>(_completedTexturePath);
            bool hovered = CenteredRectangle(CanvasPosition, _completedTexture.Size()).Contains(mousePosition.ToPoint());

            string tooltip = Quest.HoverTooltip;
            if (hovered && tooltip != null)
                mouseTooltip = tooltip;

            return hovered;
        }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            if (selected)
                DrawOutline(spriteBatch, Color.Yellow);

            else if (hovered && HasInfoPage)
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

        protected void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            Vector2 drawPos = CanvasPosition - BasicQuestLogStyle.QuestAreaOffset;
            spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.MediumQuest, iconArea);

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

        public override bool HasInfoPage => Quest.HasInfoPage;

        public override void DrawInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition, ref Action updateAction)
        {
            var quest = Quest;

            // Custom info page drawing, if overridden
            if (LoaderUtils.HasOverride(quest, q => q.DrawCustomInfoPage))
            {
                quest.DrawCustomInfoPage(spriteBatch, mousePosition);
                return;
            }

            // Get info parameters
            quest.MakeSimpleInfoPage(out var title, out var contents, out var texture);

            if (title is null && contents is null)
                return;

            // Default page drawing
            Rectangle titleArea = new(8, 10, 430, 64);
            //spriteBatch.DrawRectangle(titleArea, Color.Black);

            Rectangle underline = titleArea.CookieCutter(new(0f, 0.6f), new(1f, 0.05f));
            spriteBatch.DrawRectangle(underline, Color.Gray, fill: true);

            spriteBatch.DrawOutlinedStringInRectangle(titleArea.CookieCutter(new(0f, 0.25f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, title, stroke: 2.3f, clipBounds: false, alignment: Utilities.TextAlignment.Left);

            Rectangle contentArea = new(8, 80, 430, 450);
            const float scale = 0.5f;

            //spriteBatch.DrawOutlinedStringInRectangle(contentArea, FontAssets.DeathText.Value, Color.White, Color.Black, contents, stroke: 1.5f, maxScale: 0.5f, alignment: Utilities.TextAlignment.Left);
            spriteBatch.DrawParagraphText(FontAssets.DeathText.Value, contentArea.Location.ToVector2(), contents, scale, (int)(contentArea.Width / scale), 50f, mousePosition, out var snippet, stroke: 1.8f);

            if (snippet is not null)
            {
                updateAction = () =>
                {
                    snippet.OnHover();

                    if (Main.mouseLeft && Main.mouseLeftRelease)
                        snippet.OnClick();
                };
            }

            //spriteBatch.DrawRectangle(contentArea, Color.Black);
        }

        public override void OnDelete()
        {
            // Clone the collection to allow modified enumeration
            foreach (var connection in Connections.ToArray())
            {
                BasicQuestLogStyle.SelectedChapter.Elements.Remove(connection);
                connection.OnDelete();
            }
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
