using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using Terraria;
using Terraria.GameContent;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    [BookTooltip("ScrollBook")]
    public class ScrollBook : BasicQuestBook
    {
        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="TabBook"/>. Assigns colors and calls <see cref="DrawBasicBook(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color bookColor = Color.White; //new(255, 255, 230, 255); //new(255, 207, 64, 255);// new(240, 100, 100, 255);
            Color outlineColor = new(175, 175, 175, 255);

            if (selected)
            {
                if (BasicQuestLogStyle.UseDesigner)
                    outlineColor = Color.Yellow;

                else
                    outlineColor = new Color(200, 200, 0, 255);
            }

            else if (hovered)
            {
                //color = Color.Lerp(color, Color.Yellow, 0.2f);
                outlineColor = Color.Lerp(outlineColor, Color.White, 0.4f);
            }

            Color textOutlineColor = new(40, 40, 40, 255);
            DrawBasicBook(spriteBatch, DisplayName, bookColor, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default book drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside that container.
        /// </summary>
        public static void DrawBasicBook(SpriteBatch spriteBatch, string text, Color bookColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            spriteBatch.Draw(QuestAssets.BookScrollBorder, area.Center(), null, outlineColor, 0f, QuestAssets.BookTabBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(QuestAssets.BookScroll, area.Center(), null, bookColor, 0f, QuestAssets.BookTab.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            if (string.IsNullOrWhiteSpace(text))
                return;

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.LinearClamp, depth, raster, effect, matrix);

            DrawBookText(spriteBatch, text, textColor, textOutlineColor, area, scale);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        /// <summary>
        /// Performs the default book text drawing code to the spritebatch. Draws the text as it should sit within the given rectangle with the specified colors.
        /// </summary>
        public static void DrawBookText(SpriteBatch spriteBatch, string text, Color textColor, Color outlineColor, Rectangle area, float scale)
        {
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.1f, right: 0.4f, top: 0.1f, bottom: 0.1f).CookieCutter(new(0f, 0.4f), Vector2.One);
            var font = FontAssets.DeathText.Value;
            spriteBatch.DrawOutlinedStringInRectangle(nameRectangle, font, textColor, outlineColor, text, alignment: Utilities.TextAlignment.Left, clipBounds: false);
        }
    }
}
