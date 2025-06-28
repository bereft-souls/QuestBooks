using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultChapters
{
    /// <summary>
    /// Represents a basic <see cref="BookChapter"/> implementation. Always visible, always unlocked.
    /// </summary>
    public class BasicChapter() : BookChapter
    {
        public override List<ChapterElement> Elements { get; set; } = [];

        public override string DisplayName => Language.GetOrRegister(NameKey).Value;

        /// <summary>
        /// The localization key used to display this line's title.
        /// </summary>
        public string NameKey;

        /// <summary>
        /// Performs the default drawing behavior for this <see cref="BasicChapter"/>. Assigns colors and calls <see cref="DrawBasicChapter(SpriteBatch, string, Color, Color, Color, Color, Rectangle, float)(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color color = Color.White;
            Color outlineColor = new(135, 135, 135, 255);

            if (selected)
            {
                if (BasicQuestLogStyle.UseDesigner)
                    outlineColor = Color.Red;

                else
                    outlineColor = new Color(200, 200, 0, 255);
            }

            else if (hovered)
                outlineColor = Color.Lerp(outlineColor, Color.White, 0.4f);

            Color textOutlineColor = new(69, 69, 69, 255);
            DrawBasicChapter(spriteBatch, DisplayName, color, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default chapter drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside the contianer.
        /// </summary>
        public static void DrawBasicChapter(SpriteBatch spriteBatch, string text, Color chapterColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            spriteBatch.Draw(QuestAssets.LogEntryBorder, area.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(QuestAssets.LogEntryBackground, area.Center(), null, chapterColor, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.LinearClamp, depth, raster, effect, matrix);

            DrawChapterText(spriteBatch, text, textColor, textOutlineColor, area, scale);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        /// <summary>
        /// Performs the default chapter text drawing code to the spritebatch. Draws the text as it should sit within the given rectangle with the specified colors.
        /// </summary>
        public static void DrawChapterText(SpriteBatch spriteBatch, string text, Color textColor, Color outlineColor, Rectangle area, float scale)
        {
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.1f, right: 0.165f, top: 0.1f, bottom: 0.1f);
            float scaleShift = InverseLerp(0.4f, 2f, scale) * 0.8f;
            float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
            Vector2 offset = new(0f, MathHelper.Lerp(2f, 11f, scaleShift) / MathHelper.Clamp(text.Length / 15f, 1f, 2f));

            var font = FontAssets.DeathText.Value;
            spriteBatch.DrawOutlinedStringInRectangle(nameRectangle, font, textColor, outlineColor, text, stroke, offset: offset, extraScale: 0.8f, alignment: Utilities.TextAlignment.Left);
        }

        public override void CloneTo(BookChapter newInstance)
        {
            if (newInstance is BasicChapter chapter)
                chapter.NameKey = NameKey;

            base.CloneTo(newInstance);
        }
    }
}
