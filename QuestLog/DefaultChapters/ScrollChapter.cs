using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using Terraria;
using Terraria.GameContent;

namespace QuestBooks.QuestLog.DefaultChapters
{
    /// <summary>
    /// Represents a basic <see cref="BookChapter"/> implementation. Always visible, always unlocked.
    /// </summary>
    [ChapterTooltip("ScrollChapter")]
    public class ScrollChapter : BasicChapter
    {
        /// <summary>
        /// Performs the default drawing behavior for this <see cref="ScrollChapter"/>. Assigns colors and calls <see cref="DrawBasicChapter(SpriteBatch, string, Color, Color, Color, Color, Rectangle, float)(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color chapterColor = Color.White;
            Color outlineColor = new(0, 0, 0, 0);

            if (selected)
                outlineColor = new Color(225, 225, 0, 255);

            else if (hovered)
                outlineColor = new(200, 200, 200, 255);

            Color textOutlineColor = new(69, 69, 69, 255);
            DrawBasicChapter(spriteBatch, DisplayName, chapterColor, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default chapter drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside the contianer.
        /// </summary>
        public static void DrawBasicChapter(SpriteBatch spriteBatch, string text, Color chapterColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            if (outlineColor.A > 0)
                spriteBatch.Draw(QuestAssets.ChapterScrollBorder, area.Center(), null, outlineColor, 0f, QuestAssets.ChapterScrollBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            spriteBatch.Draw(QuestAssets.ChapterScroll, area.Center(), null, chapterColor, 0f, QuestAssets.ChapterScroll.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

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
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.09f, right: 0.32f);//.CreateScaledMargins(left: 0.1f, right: 0.165f, top: 0.1f, bottom: 0.1f);

            var font = FontAssets.DeathText.Value;
            spriteBatch.DrawOutlinedStringInRectangle(nameRectangle.CookieCutter(new(0f, 0.24f), Vector2.One), font, textColor, outlineColor, text, alignment: Utilities.TextAlignment.Left, clipBounds: false, maxScale: 0.5f);
        }
    }
}
