using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    /// <summary>
    /// Represents a basic <see cref="QuestBook"/> implementation. Always visible, always unlocked.
    /// </summary>
    public class BasicQuestBook() : QuestBook
    {
        public override List<BookChapter> Chapters { get; set; } = [];

        public override string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        public override void Update()
        {
            foreach (BookChapter questLine in Chapters)
                questLine.Update();
        }

        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="BasicQuestBook"/>. Assigns colors and calls <see cref="DrawBasicBook(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color color = Color.SlateGray;

            if (!selected)
                color = Color.Lerp(color, Color.Black, 0.25f);

            if (hovered)
                color = Color.Lerp(color, Color.White, 0.1f);

            Color outlineColor = BasicQuestLogStyle.UseDesigner && selected ? Color.Red : Color.Lerp(color, Color.Black, 0.2f);
            Color textOutlineColor = Color.Lerp(color, Color.Black, 0.4f);
            DrawBasicBook(spriteBatch, DisplayName, color, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default book drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside that container.
        /// </summary>
        public static void DrawBasicBook(SpriteBatch spriteBatch, string text, Color bookColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            spriteBatch.Draw(QuestAssets.LogEntryBackground, area.Center(), null, bookColor, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(QuestAssets.LogEntryBorder, area.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

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
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.065f, right: 0.065f, top: 0.1f, bottom: 0.1f);
            float scaleShift = InverseLerp(0.4f, 2f, scale) * 0.8f;
            float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
            Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift) / MathHelper.Clamp(text.Length / 15f, 1f, 2f));

            var font = FontAssets.DeathText.Value;
            var (line, drawPos, origin, textScale) = GetRectangleStringParameters(nameRectangle, font, text, offset: offset, alignment: Utilities.TextAlignment.Left)[0];
            textScale *= 0.8f;

            spriteBatch.DrawOutlinedString(font, line, drawPos, origin, textScale, stroke, outlineColor, textColor);
        }

        /// <summary>
        /// Clones the members of this quest book into a new quest book.
        /// </summary>
        public override void CloneTo(QuestBook newInstance)
        {
            if (newInstance is BasicQuestBook book)
                book.NameKey = NameKey;

            base.CloneTo(newInstance);
        }
    }
}
