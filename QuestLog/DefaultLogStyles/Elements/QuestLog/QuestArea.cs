using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        // Handles the display of a selected questline element
        public static ChapterElement SelectedElement { get; set; } = null;
        public static ChapterElement HoveredElement { get; set; } = null;

        private static float questElementSwipeOffset = 0f;
        public static Vector2 QuestAreaOffset = Vector2.Zero;
        private static Vector2 minQuestAreaOffset => UseDesigner ? new(float.MinValue) : SelectedChapter.MinViewPoint;
        private static Vector2 maxQuestAreaOffset => UseDesigner ? new(float.MaxValue) : SelectedChapter.MaxViewPoint;

        private static Vector2? cachedRightClick = null;
        private static Vector2 cachedOffset = Vector2.Zero;

        private void UpdateQuestArea(Rectangle questArea, Vector2 scaledMouse)
        {
            // These are the things like placing new elements or region toggles
            if (UseDesigner)
                HandleQuestRegionTools();

            bool mouseInBounds = questArea.Contains(scaledMouse.ToPoint());
            SwitchTargets(questAreaTarget, ContentBlending);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Transparent));

            if ((mouseInBounds || cachedRightClick is not null) && (SelectedChapter?.EnableShifting ?? false))
            {
                if (RightMouseJustPressed)
                {
                    cachedRightClick = scaledMouse;
                    cachedOffset = QuestAreaOffset;
                }

                else if (RightMouseHeld)
                    QuestAreaOffset = cachedOffset - ((scaledMouse - (cachedRightClick ?? Vector2.Zero)) / TargetScale);

                else if (RightMouseJustReleased)
                {
                    cachedRightClick = null;
                    cachedOffset = Vector2.Zero;
                }
            }

            if (SelectedChapter?.EnableShifting ?? false)
            {
                QuestAreaOffset = new(float.Min(QuestAreaOffset.X, maxQuestAreaOffset.X), float.Min(QuestAreaOffset.Y, maxQuestAreaOffset.Y));
                QuestAreaOffset = new(float.Max(QuestAreaOffset.X, minQuestAreaOffset.X), float.Max(QuestAreaOffset.Y, minQuestAreaOffset.Y));
            }

            // Scale based on target size
            // This makes sure that no matter the scale, the canvas has the same "size" for elements to draw to
            Matrix transform = Matrix.CreateTranslation(questElementSwipeOffset, 0f, 0f) * Matrix.CreateScale(TargetScale);
            Vector2 placementPosition = scaledMouse / TargetScale + QuestAreaOffset;

            // Switch draw matrices
            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix * transform);
            });

            // This does the backdrop, grid, etc
            if (UseDesigner)
                DesignerPreQuestRegion(placementPosition);

            // Sort the elements if requested
            SortedElements ??= SelectedChapter?.Elements.OrderBy(x => x.DrawPriority).ToArray() ?? null;

            // Get the top-most element that is being hovered            
            ChapterElement lastHoveredElement = mouseInBounds ? SortedElements?.LastOrDefault(x => x.IsHovered(placementPosition, ref MouseTooltip), null) ?? null : null;
            HoveredElement = lastHoveredElement;

            if (LeftMouseJustReleased && (lastHoveredElement is not null || (lastHoveredElement is null && SelectedElement is not null && mouseInBounds)) && placingElement is null && !movingAnchor && !movingMaxView)
            {
                ChapterElement element = lastHoveredElement == SelectedElement ? null : lastHoveredElement;
                swipingBetweenInfoPages = element is not null && SelectedElement is not null;
                SelectedElement = (element?.HasInfoPage ?? false) || UseDesigner ? element : null;

                if ((element?.HasInfoPage ?? false) || UseDesigner)
                    questInfoSwipeOffset = questInfoTarget.Height * (element is null ? -1 : 1);
            }

            if (questElementSwipeOffset != 0f)
            {
                questElementSwipeOffset = MathHelper.Lerp(questElementSwipeOffset, 0f, 0.22f);

                if (Math.Abs(questElementSwipeOffset) < 0.5f)
                    questElementSwipeOffset = 0f;
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                sb.Begin(SpriteSortMode.Deferred, blend, SamplerState.PointClamp, depth, raster, effect, matrix);

                // Draw elements
                if (SortedElements is not null)
                    foreach (var element in SortedElements.Where(x => x.VisibleOnCanvas()))
                        element.DrawToCanvas(sb, QuestAreaOffset, SelectedElement == element, lastHoveredElement == element);

                sb.End();

                // Draw the previously-rendered target along with the new elements if being swiped
                if (questElementSwipeOffset != 0f)
                {
                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, Matrix.Identity);
                    float xOffset = -float.Sign(questElementSwipeOffset) * (previousQuestAreaTarget.Width - Math.Abs(questElementSwipeOffset));
                    sb.Draw(previousQuestAreaTarget, new Vector2(xOffset, 0f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    return;
                }

                // Draw our content to a secondary target so that we can swipe when new elements are selected
                sb.GraphicsDevice.SetRenderTarget(previousQuestAreaTarget);
                sb.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, TargetCopying, sampler, depth, raster, effect, Matrix.Identity);

                sb.Draw(questAreaTarget, Vector2.Zero, Color.White);

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(questAreaTarget);
                sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

            // This is things like the placement preview and actually placing elements
            if (UseDesigner)
                DesignerPostQuestRegion(placementPosition, mouseInBounds);

            SwitchTargets(null);
        }
    }
}
