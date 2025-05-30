﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace QuestBooks.Utilities
{
    public enum TextAlignment
    {
        Left,
        Middle,
        Right
    }

    public static partial class Utils
    {
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float stroke = 2f, bool fill = false)
        {
            Texture2D pixel = QuestAssets.MagicPixel;

            if (fill)
            {
                spriteBatch.Draw(pixel, rectangle, color);
                return;
            }

            int halfStroke = (int)Math.Ceiling(stroke * 0.5f);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left - halfStroke, rectangle.Top - halfStroke, rectangle.Width + (int)stroke, (int)stroke), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left - halfStroke, rectangle.Top - halfStroke, (int)stroke, rectangle.Height + (int)stroke), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left - halfStroke, rectangle.Bottom - halfStroke, rectangle.Width + (int)stroke, (int)stroke), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Right - halfStroke, rectangle.Top - halfStroke, (int)stroke, rectangle.Height + (int)stroke), color);
        }

        public static List<(string line, Vector2 drawPos, Vector2 origin, float scale)> GetRectangleStringParameters(
            Rectangle rectangle,
            DynamicSpriteFont font,
            string text,
            float? minimumScale = null,
            float? maxScale = null,
            float? extraScale = null,
            TextAlignment alignment = TextAlignment.Middle,
            Vector2? offset = null,
            float verticalSpacing = 0)
        {
            string[] strings = text.Split('\n');
            List<(string line, Vector2 drawPos, Vector2 origin, float scale)> results = [];

            if (strings.Length == 0)
                return results;

            float availableSpace = rectangle.Height - ((strings.Length - 1) * verticalSpacing);
            float lineHeight = availableSpace / strings.Length;

            for (int i = 0; i < strings.Length; i++)
            {
                string line = strings[i];
                Rectangle designatedArea = new(rectangle.X, (int)(rectangle.Y + (i * (verticalSpacing + lineHeight))), rectangle.Width, (int)lineHeight);

                Vector2 textSize = font.MeasureString(line);
                Vector2 ratio = designatedArea.Size() / textSize;

                float scale = Math.Min(ratio.X, ratio.Y);
                if (minimumScale.HasValue)
                    scale = Math.Max(scale, minimumScale.Value);

                if (maxScale.HasValue)
                    scale = Math.Min(scale, maxScale.Value);

                Vector2 origin = alignment switch
                {
                    TextAlignment.Left => new Vector2(0, textSize.Y * 0.5f),
                    TextAlignment.Middle => textSize * 0.5f,
                    TextAlignment.Right => new Vector2(textSize.X, textSize.Y * 0.5f),
                    _ => textSize * 0.5f
                };

                Vector2 drawPos = alignment switch
                {
                    TextAlignment.Left => new(designatedArea.Left, designatedArea.Center().Y),
                    TextAlignment.Middle => designatedArea.Center(),
                    TextAlignment.Right => new(designatedArea.Right, designatedArea.Center().Y),
                    _ => designatedArea.Center()

                };

                if (offset.HasValue)
                    drawPos += offset.Value;

                if (extraScale.HasValue)
                    scale *= extraScale.Value;

                results.Add(new(line, drawPos, origin, scale));
            }

            return results;
        }

        public static void DrawStringInRectangle(this SpriteBatch spriteBatch,
            Rectangle rectangle,
            DynamicSpriteFont font,
            Color color,
            string text,
            float? minimumScale = null,
            float? maxScale = null,
            float extraScale = 1f,
            TextAlignment alignment = TextAlignment.Middle,
            Vector2? offset = null,
            float verticalSpacing = 0)
        {
            var results = GetRectangleStringParameters(rectangle, font, text, minimumScale, maxScale, extraScale, alignment, offset, verticalSpacing);

            foreach (var (line, drawPos, origin, scale) in results)
                spriteBatch.DrawString(font, line, drawPos, color, 0f, origin, scale * extraScale, SpriteEffects.None, 0f);
        }
    }
}
