using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameInput;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private float questElementSwipeOffset = 0f;
        private Vector2 minQuestAreaOffset => UseDesigner ? new(float.MinValue) : SelectedChapter.MinViewPoint;
        private Vector2 maxQuestAreaOffset => UseDesigner ? new(float.MaxValue) : SelectedChapter.MaxViewPoint + defaultCanvasSize * (1 - 1 / Zoom);

        /// The minimum zoom level that fits the entire canvas in the viewport
        private float MinFitZoom
        {
            get
            {
                if (UseDesigner || SelectedChapter is null)
                    return 0.1f;

                Vector2 canvasSize = SelectedChapter.MaxViewPoint + defaultCanvasSize - SelectedChapter.MinViewPoint;
                float fitZoomX = defaultCanvasSize.X / canvasSize.X;
                float fitZoomY = defaultCanvasSize.Y / canvasSize.Y;
                return float.Max(float.Min(fitZoomX, fitZoomY), 0.1f);
            }
        }

        /// The offset that centers the canvas in the viewport at the current zoom level
        private Vector2 CenteredOffset
        {
            get
            {
                if (SelectedChapter is null)
                    return Vector2.Zero;

                Vector2 canvasSize = (SelectedChapter.MaxViewPoint + defaultCanvasSize) - SelectedChapter.MinViewPoint;
                Vector2 canvasCenter = SelectedChapter.MinViewPoint + canvasSize / 2;
                Vector2 visibleArea = defaultCanvasSize / Zoom;
                return canvasCenter - visibleArea / 2;
            }
        }

        /// Returns which axes can pan at the current zoom level (canvas extends beyond viewport)
        /// X component is 1 if horizontal panning is allowed, 0 otherwise
        /// Y component is 1 if vertical panning is allowed, 0 otherwise
        private (bool canPanX, bool canPanY) CanPanAxes
        {
            get
            {
                if (UseDesigner || SelectedChapter is null)
                    return (true, true);

                Vector2 canvasSize = (SelectedChapter.MaxViewPoint + defaultCanvasSize) - SelectedChapter.MinViewPoint;
                Vector2 visibleArea = defaultCanvasSize / Zoom;

                return (canvasSize.X > visibleArea.X, canvasSize.Y > visibleArea.Y);
            }
        }

        private Vector2? cachedRightClick = null;
        private Vector2 cachedOffset = Vector2.Zero;

        private void UpdateQuestArea(Rectangle questArea, Vector2 scaledMouse)
        {
            // These are the things like placing new elements or region toggles
            if (UseDesigner)
                HandleQuestRegionTools();

            bool mouseInBounds = questArea.Contains(scaledMouse.ToPoint());
            SwitchTargets(questAreaTarget, BlendState.AlphaBlend);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Transparent));

            var (canPanX, canPanY) = CanPanAxes;
            bool canPan = UseDesigner || canPanX || canPanY;

            if ((mouseInBounds || cachedRightClick is not null) && (SelectedChapter?.EnableShifting ?? false) && canPan)
            {
                if (RightMouseJustPressed)
                {
                    cachedRightClick = scaledMouse;
                    cachedOffset = QuestAreaOffset;
                }

                else if (RightMouseHeld)
                {
                    Vector2 delta = ((scaledMouse - (cachedRightClick ?? Vector2.Zero)) / TargetScale) / Zoom;

                    // Only apply panning on axes that allow it (non-designer mode)
                    if (!UseDesigner)
                    {
                        if (!canPanX)
                            delta.X = 0;

                        if (!canPanY)
                            delta.Y = 0;
                    }

                    QuestAreaOffset = cachedOffset - delta;
                }

                else if (RightMouseJustReleased)
                {
                    cachedRightClick = null;
                    cachedOffset = Vector2.Zero;
                }
            }

            int scrollAmount = PlayerInput.ScrollWheelDeltaForUI / 6;

            if (mouseInBounds && scrollAmount != 0 && cachedRightClick is null && (SelectedChapter?.EnableShifting ?? false))
            {
                float oldZoom = Zoom;
                Zoom += scrollAmount * 0.005f;
                Zoom = float.Min(Zoom, 2f);
                Zoom = float.Max(Zoom, MinFitZoom);

                // Adjust offset to keep the mouse's canvas position stable
                QuestAreaOffset += scaledMouse / TargetScale * (1 / oldZoom - 1 / Zoom);
            }

            if (SelectedChapter?.EnableShifting ?? false)
            {
                // Clamp zoom to minimum fit zoom
                if (!UseDesigner && Zoom <= MinFitZoom)
                    Zoom = MinFitZoom;

                if (UseDesigner)
                {
                    QuestAreaOffset = new(float.Min(QuestAreaOffset.X, maxQuestAreaOffset.X), float.Min(QuestAreaOffset.Y, maxQuestAreaOffset.Y));
                    QuestAreaOffset = new(float.Max(QuestAreaOffset.X, minQuestAreaOffset.X), float.Max(QuestAreaOffset.Y, minQuestAreaOffset.Y));
                }
                else
                {
                    // Center on axes where canvas fits, clamp on axes where it extends
                    Vector2 centered = CenteredOffset;
                    (canPanX, canPanY) = CanPanAxes;

                    float offsetX = canPanX
                        ? float.Clamp(QuestAreaOffset.X, minQuestAreaOffset.X, maxQuestAreaOffset.X)
                        : centered.X;
                    float offsetY = canPanY
                        ? float.Clamp(QuestAreaOffset.Y, minQuestAreaOffset.Y, maxQuestAreaOffset.Y)
                        : centered.Y;
                    QuestAreaOffset = new(offsetX, offsetY);
                }
            }

            // Scale based on target size
            // This makes sure that no matter the scale, the canvas has the same "size" for elements to draw to
            Matrix transform = Matrix.CreateTranslation(questElementSwipeOffset, 0f, 0f) * Matrix.CreateScale(TargetScale);
            Vector2 placementPosition = scaledMouse / TargetScale / Zoom + QuestAreaOffset;

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
            ChapterElement lastHoveredElement = mouseInBounds ? SortedElements?.LastOrDefault(x => x.IsHovered(placementPosition, QuestAreaOffset, Zoom, ref MouseTooltip) && x != SelectedElement, null) ?? null : null;
            HoveredElement = lastHoveredElement;

            if (LeftMouseJustReleased && (lastHoveredElement is not null || (lastHoveredElement is null && SelectedElement is not null && mouseInBounds)) && placingElement is null && !movingAnchor && !movingMaxView)
            {
                ChapterElement element = lastHoveredElement == SelectedElement ? null : lastHoveredElement;

                if (!UseDesigner)
                    element?.OnSelect();

                element = ((element?.HasInfoPage ?? false) || UseDesigner) ? element : null;
                swipingBetweenInfoPages = element is not null && SelectedElement is not null;
                bool swiping = swipingBetweenInfoPages || (element is null != SelectedElement is null);
                SelectedElement = (element?.HasInfoPage ?? false) || UseDesigner ? element : null;

                if (swiping)
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
                        element.DrawToCanvas(sb, QuestAreaOffset, Zoom, SelectedElement == element, lastHoveredElement == element);

                sb.End();

                // Draw the previously-rendered target along with the new elements if being swiped
                if (questElementSwipeOffset != 0f)
                {
                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
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
