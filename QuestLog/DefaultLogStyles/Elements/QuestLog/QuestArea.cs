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

        private static float questElementSwipeOffset = 0f;
        private static Vector2 questAreaOffset = Vector2.Zero;

        private void UpdateQuestArea(Rectangle questArea, Vector2 scaledMouse)
        {
            // These are the things like placing new elements or region toggles
            if (UseDesigner)
                HandleQuestRegionTools();

            SwitchTargets(questAreaTarget, ContentBlending);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Transparent));

            // Scale based on target size
            // This makes sure that no matter the scale, the canvas has the same "size" for elements to draw to
            Matrix transform = Matrix.CreateTranslation(questElementSwipeOffset, 0f, 0f) * Matrix.CreateScale(TargetScale);
            Vector2 placementPosition = scaledMouse / TargetScale + questAreaOffset;

            // Switch draw matrices
            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix * transform);
            });

            // This does the backdrop, grid, etc
            if (UseDesigner)
                DesignerPreQuestRegion(placementPosition);

            // Sort the elements if requested
            SortedElements ??= SelectedChapter?.Elements.OrderBy(x => x.DrawPriority).ToArray() ?? null;

            // Get the top-most element that is being hovered
            ChapterElement lastHoveredElement = SortedElements?.LastOrDefault(x => x.IsHovered(placementPosition), null) ?? null;
            if (lastHoveredElement is not null && LeftMouseJustReleased)
            {
                ChapterElement element = lastHoveredElement == SelectedElement ? null : lastHoveredElement;
                SelectedElement = element;
            }

            DrawTasks.Add(sb =>
            {
                // Draw elements
                if (SortedElements is not null)
                    foreach (var element in SortedElements)
                        element.DrawToCanvas(sb, questAreaOffset, lastHoveredElement == element);

                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                // Draw the previously-rendered target along with the new elements if being swiped
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

                // Draw our content to a secondary target so that we can swipe when new elements are selected
                sb.GraphicsDevice.SetRenderTarget(previousQuestAreaTarget);
                sb.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, Matrix.Identity);

                sb.Draw(questAreaTarget, Vector2.Zero, Color.White);

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(questAreaTarget);
                sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

            // This is things like the placement preview and actually placing elements
            if (UseDesigner)
                DesignerPostQuestRegion(placementPosition, questArea.Contains(scaledMouse.ToPoint()));

            SwitchTargets(null);
        }
    }
}
