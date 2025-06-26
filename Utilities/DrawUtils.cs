using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        private static readonly FieldInfo customEffectField = typeof(SpriteBatch).GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo transformMatrixField = typeof(SpriteBatch).GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void GetDrawParameters(this SpriteBatch spriteBatch,
            out BlendState blendState,
            out SamplerState samplerState,
            out DepthStencilState depthStencilState,
            out RasterizerState rasterizerState,
            out Effect effect,
            out Matrix matrix)
        {
            blendState = spriteBatch.GraphicsDevice.BlendState;
            samplerState = spriteBatch.GraphicsDevice.SamplerStates[0];
            depthStencilState = spriteBatch.GraphicsDevice.DepthStencilState;
            rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
            effect = customEffectField.GetValue(spriteBatch) as Effect;
            matrix = (Matrix)transformMatrixField.GetValue(spriteBatch);
        }

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
        public static void DrawOutlinedStringInRectangle(this SpriteBatch spriteBatch,
            Rectangle rectangle,
            DynamicSpriteFont font,
            Color color,
            Color outlineColor,
            string text,
            float stroke = 2f,
            float? minimumScale = null,
            float? maxScale = null,
            float extraScale = 1f,
            TextAlignment alignment = TextAlignment.Middle,
            Vector2? offset = null,
            float verticalSpacing = 0,
            bool clipBounds = true)
        {
            var results = GetRectangleStringParameters(rectangle, font, text, minimumScale, maxScale, extraScale, alignment, offset, verticalSpacing);

            bool cachedClip = spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable;
            Rectangle cachedClipArea = spriteBatch.GraphicsDevice.ScissorRectangle;

            if (clipBounds)
            {
                spriteBatch.End();
                spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                raster.ScissorTestEnable = true;
                spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(cachedClipArea, rectangle);

                spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            }

            if (spriteBatch.GraphicsDevice.ScissorRectangle != Rectangle.Empty)
                foreach (var (line, drawPos, origin, scale) in results)
                    spriteBatch.DrawOutlinedString(font, line, drawPos, origin, scale, stroke, outlineColor, color);

            if (clipBounds)
            {
                spriteBatch.End();
                spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                raster.ScissorTestEnable = cachedClip;
                spriteBatch.GraphicsDevice.ScissorRectangle = cachedClipArea;

                spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            }
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
            float verticalSpacing = 0,
            bool clipBounds = true)
        {
            var results = GetRectangleStringParameters(rectangle, font, text, minimumScale, maxScale, extraScale, alignment, offset, verticalSpacing);

            bool cachedClip = spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable;
            Rectangle cachedClipArea = spriteBatch.GraphicsDevice.ScissorRectangle;

            if (clipBounds)
            {
                spriteBatch.End();
                spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                raster.ScissorTestEnable = true;
                spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(cachedClipArea, rectangle);

                spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            }

            if (spriteBatch.GraphicsDevice.ScissorRectangle != Rectangle.Empty)
                foreach (var (line, drawPos, origin, scale) in results)
                    spriteBatch.DrawString(font, line, drawPos, color, 0f, origin, scale, SpriteEffects.None, 0f);

            if (clipBounds)
            {
                spriteBatch.End();
                spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                raster.ScissorTestEnable = cachedClip;
                spriteBatch.GraphicsDevice.ScissorRectangle = cachedClipArea;

                spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            }
        }

        public static void DrawOutlinedString(this SpriteBatch spriteBatch,
            SpriteFont font,
            string line,
           Vector2 drawPos,
           Vector2 origin,
           float scale,
           float stroke = 2f,
           Color? outlineColor = null,
           Color? textColor = null)
        {
            Color strokeColor = outlineColor ?? Color.Black;
            Color color = textColor ?? Color.White;

            spriteBatch.DrawString(font, line, drawPos + new Vector2(-stroke, 0f), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(stroke, 0f), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(0f, stroke), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(0f, -stroke), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public static void DrawOutlinedString(this SpriteBatch spriteBatch,
            DynamicSpriteFont font,
            string line,
           Vector2 drawPos,
           Vector2 origin,
           float scale,
           float stroke = 2f,
           Color? outlineColor = null,
           Color? textColor = null)
        {
            Color strokeColor = outlineColor ?? Color.Black;
            Color color = textColor ?? Color.White;

            spriteBatch.DrawString(font, line, drawPos + new Vector2(-stroke, 0f), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(stroke, 0f), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(0f, stroke), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos + new Vector2(0f, -stroke), strokeColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, line, drawPos, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
