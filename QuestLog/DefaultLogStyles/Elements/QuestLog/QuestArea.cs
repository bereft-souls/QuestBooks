using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        // Handles the display of a selected questline element
        public static ChapterElement SelectedElement { get; set; } = null;
        private static ChapterElement[] SortedElements = null;

        private static float questElementSwipeOffset = 0f;
        private static Vector2 QuestAreaOffset = Vector2.Zero;

        private void UpdateQuestArea(Rectangle questArea, Vector2 scaledMouse)
        {
            if (UseDesigner)
                HandleQuestRegionTools();

            SwitchTargets(questAreaTarget, ContentBlending);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Transparent));

            Matrix transform = Matrix.CreateTranslation(questElementSwipeOffset, 0f, 0f) * Matrix.CreateScale(targetScale);
            Vector2 placementPosition = scaledMouse / targetScale;

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix * transform);
            });

            if (UseDesigner)
                DesignerPreQuestRegion(placementPosition);

            SortedElements ??= SelectedChapter?.Elements.OrderBy(x => x.DrawPriority).ToArray() ?? null;

            ChapterElement lastHoveredElement = SortedElements?.LastOrDefault(x => x.IsHovered(placementPosition), null) ?? null;
            if (lastHoveredElement is not null && LeftMouseJustReleased)
            {
                ChapterElement element = lastHoveredElement == SelectedElement ? null : lastHoveredElement;
                SelectedElement = element;
            }

            DrawTasks.Add(sb =>
            {
                if (SortedElements is not null)
                    foreach (var element in SortedElements)
                        element.DrawToCanvas(sb, QuestAreaOffset, lastHoveredElement == element);

                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                if (questElementSwipeOffset != 0f)
                {
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, Matrix.Identity);
                    float xOffset = -float.Sign(questElementSwipeOffset) * (previousQuestAreaTarget.Width - Math.Abs(questElementSwipeOffset));
                    sb.Draw(previousQuestAreaTarget, new Vector2(xOffset, 0f), null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);

                    questElementSwipeOffset = MathHelper.Lerp(questElementSwipeOffset, 0f, 0.2f);

                    if (Math.Abs(questElementSwipeOffset) < 2f)
                        questElementSwipeOffset = 0f;

                    return;
                }

                sb.GraphicsDevice.SetRenderTarget(previousQuestAreaTarget);
                sb.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, Matrix.Identity);

                sb.Draw(questAreaTarget, Vector2.Zero, Color.White);

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(questAreaTarget);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

            if (UseDesigner)
                DesignerPostQuestRegion(placementPosition, questArea.Contains(scaledMouse.ToPoint()));

            SwitchTargets(null);
        }
    }
}
