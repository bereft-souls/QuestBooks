﻿using Microsoft.Xna.Framework;
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
            var results = GetRectangleStringParameters(rectangle.CreateMargin((int)(stroke * 0.5f)), font, text, minimumScale, maxScale, extraScale, alignment, offset, verticalSpacing);

            bool cachedClip = spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable;
            Rectangle cachedClipArea = spriteBatch.GraphicsDevice.ScissorRectangle;

            if (clipBounds)
            {
                spriteBatch.End();
                spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                raster.ScissorTestEnable = true;
                matrix.Decompose(out var scale, out _, out var translation);

                rectangle = new(
                    (int)(rectangle.X * scale.X + translation.X),
                    (int)(rectangle.Y * scale.Y + translation.Y),
                    (int)(rectangle.Width * scale.X),
                    (int)(rectangle.Height * scale.Y));

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
                matrix.Decompose(out var scale, out _, out var translation);

                rectangle = new(
                    (int)(rectangle.X * scale.X + translation.X),
                    (int)(rectangle.Y * scale.Y + translation.Y),
                    (int)(rectangle.Width * scale.X),
                    (int)(rectangle.Height * scale.Y));

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

        /// <summary>
        /// Draws a 9-patch rectangle to the specified rectangle with the specified color.
        /// </summary>
        public static void DrawPatchRectangle(this SpriteBatch spriteBatch, PatchRectangle patchRectangle, Rectangle rectangle, Color? color = null)
        {
            Color drawColor = color ?? Color.White;

            int rightStart = patchRectangle.Asset.Width - patchRectangle.Right;
            int bottomStart = patchRectangle.Asset.Height - patchRectangle.Bottom;

            Rectangle patchTopLeft = new(0, 0, patchRectangle.Left, patchRectangle.Top);
            Rectangle patchTopRight = new(rightStart, 0, patchRectangle.Right, patchRectangle.Top);
            Rectangle patchBottomRight = new(rightStart, bottomStart, patchRectangle.Right, patchRectangle.Bottom);
            Rectangle patchBottomLeft = new(0, bottomStart, patchRectangle.Left, patchRectangle.Bottom);

            int edgeHeight = patchRectangle.Asset.Height - patchRectangle.Bottom - patchRectangle.Top;
            int edgeWidth = patchRectangle.Asset.Width - patchRectangle.Right - patchRectangle.Left;

            Rectangle patchTop = new(patchRectangle.Left, 0, edgeWidth, patchRectangle.Top);
            Rectangle patchRight = new(rightStart, patchRectangle.Top, patchRectangle.Right, edgeHeight);
            Rectangle patchBottom = new(patchRectangle.Left, bottomStart, edgeWidth, patchRectangle.Bottom);
            Rectangle patchLeft = new(0, patchRectangle.Top, patchRectangle.Left, edgeHeight);

            Rectangle patchCenter = new(patchRectangle.Left, patchRectangle.Top, edgeWidth, edgeHeight);

            Rectangle targetCenter = new(
                rectangle.Location.X + patchRectangle.Left,
                rectangle.Location.Y + patchRectangle.Top,
                rectangle.Width - patchRectangle.Right - patchRectangle.Left,
                rectangle.Height - patchRectangle.Bottom - patchRectangle.Top);

            Rectangle targetTop = new(targetCenter.Left, rectangle.Top, targetCenter.Width, patchRectangle.Top);
            Rectangle targetRight = new(targetCenter.Right, targetCenter.Top, patchRectangle.Right, targetCenter.Height);
            Rectangle targetBottom = new(targetCenter.Left, targetCenter.Bottom, targetCenter.Width, patchRectangle.Bottom);
            Rectangle targetLeft = new(rectangle.Left, targetCenter.Top, patchRectangle.Left, targetCenter.Height);

            Vector2 targetTopLeft = rectangle.Location.ToVector2();
            Vector2 targetTopRight = new(targetCenter.Right, rectangle.Top);
            Vector2 targetBottomRight = new(targetCenter.Right, targetCenter.Bottom);
            Vector2 targetBottomLeft = new(rectangle.Left, targetCenter.Bottom);

            spriteBatch.Draw(patchRectangle, targetCenter, patchCenter, drawColor);

            // Draw edges in clockwise order
            if (patchRectangle.RepeatEdges)
            {
                for (int i = 0; i < targetTop.Width; i += patchTop.Height)
                {
                    int drawPos = targetTop.Left + i;
                    int targetWidth = targetTop.Right - drawPos;

                    patchTop.Width = Math.Min(patchTop.Width, targetWidth);
                    spriteBatch.Draw(patchRectangle, new Vector2(drawPos, targetTop.Top), patchTop, drawColor);
                }

                for (int i = 0; i < targetRight.Height; i += patchRight.Height)
                {
                    int drawPos = targetRight.Top + i;
                    int targetHeight = targetRight.Bottom - drawPos;

                    patchRight.Height = Math.Min(patchRight.Height, targetHeight);
                    spriteBatch.Draw(patchRectangle, new Vector2(targetLeft.Left, drawPos), patchRight, drawColor);
                }

                for (int i = targetBottom.Width - patchBottom.Width; i > -patchBottom.Width; i -= patchBottom.Width)
                {
                    int drawPos = targetBottom.Left + i;
                    int targetWidth = drawPos + patchBottom.Width;

                    if (targetWidth < patchBottom.Width)
                    {
                        patchBottom.X += patchBottom.Width - targetWidth;
                        patchBottom.Width = targetWidth;
                        drawPos = targetBottom.Left;
                    }

                    spriteBatch.Draw(patchRectangle, new Vector2(drawPos, targetBottom.Top), patchBottom, drawColor);
                }

                for (int i = targetLeft.Height - patchLeft.Height; i > -patchLeft.Height; i -= patchLeft.Height)
                {
                    int drawPos = targetLeft.Top + i;
                    int targetHeight = drawPos + patchLeft.Height;

                    if (targetHeight < patchLeft.Height)
                    {
                        patchLeft.Y += patchLeft.Height - targetHeight;
                        patchLeft.Height = targetHeight;
                        drawPos = targetLeft.Top;
                    }

                    spriteBatch.Draw(patchRectangle, new Vector2(targetLeft.Left, drawPos), patchLeft, drawColor);
                }
            }

            else
            {
                spriteBatch.Draw(patchRectangle, targetTop, patchTop, drawColor);
                spriteBatch.Draw(patchRectangle, targetRight, patchRight, drawColor);
                spriteBatch.Draw(patchRectangle, targetBottom, patchBottom, drawColor);
                spriteBatch.Draw(patchRectangle, targetLeft, patchLeft, drawColor);
            }

            spriteBatch.Draw(patchRectangle, targetTopLeft, patchTopLeft, drawColor);
            spriteBatch.Draw(patchRectangle, targetTopRight, patchTopRight, drawColor);
            spriteBatch.Draw(patchRectangle, targetBottomRight, patchBottomRight, drawColor);
            spriteBatch.Draw(patchRectangle, targetBottomLeft, patchBottomLeft, drawColor);
        }
    }
}
