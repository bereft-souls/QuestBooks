using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using System;
using Terraria;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private bool showBackdrop = false;
        private bool showGrid = false;
        private bool snapToGrid = false;
        private int gridSize = 20;

        private bool moveBounds = true;
        private bool showMidpoint = true;
        private bool movingAnchor = false;
        private bool movingMinView = false;
        private bool movingMaxView = false;

        private Vector2 chapterAnchor = Vector2.Zero;
        private Vector2 chapterMinView = Vector2.Zero;
        private Vector2 chapterMaxView = Vector2.Zero;

        private static readonly Vector2 defaultAnchor = new(230f, 270f);
        private static readonly Vector2 defaultCanvasSize = new(460, 540);

        private void DesignerPreQuestRegion(Vector2 mousePosition)
        {
            if (SelectedChapter is null)
                return;

            if (showBackdrop)
                DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.15f));

            if (showGrid)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.Begin(SpriteSortMode.Deferred, GridBlending, sampler, depth, raster, effect, matrix);
                });

                Color gridColor = Color.White with { A = (byte)(showBackdrop ? 0 : 100) };
                float scaledGridSize = gridSize * Zoom;

                for (float xPos = 0; xPos < 500; xPos += scaledGridSize)
                {
                    Vector2 drawCenter = new(xPos - (QuestAreaOffset.X % gridSize) * Zoom, 300f);
                    Rectangle rect = CenteredRectangle(drawCenter, new(1f, 600f));
                    AddRectangle(rect, gridColor);
                }

                for (float yPos = 0; yPos < 600; yPos += scaledGridSize)
                {
                    Vector2 drawCenter = new(300f, yPos - (QuestAreaOffset.Y % gridSize) * Zoom);
                    Rectangle rect = CenteredRectangle(drawCenter, new(600f, 1f));
                    AddRectangle(rect, gridColor);
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, depth, raster, effect, matrix);
                });
            }
        }

        private void DesignerPostQuestRegion(Vector2 mousePosition, bool mouseInBounds)
        {
            // Round to grid intersections
            Vector2 unsnapped = (mousePosition - QuestAreaOffset) * Zoom;
            if (snapToGrid)
            {
                mousePosition.X = float.Round(mousePosition.X / gridSize, MidpointRounding.AwayFromZero) * gridSize;
                mousePosition.Y = float.Round(mousePosition.Y / gridSize, MidpointRounding.AwayFromZero) * gridSize;
            }

            // Otherwise round to integers (helps with drawing)
            else
            {
                mousePosition.X = float.Round(mousePosition.X, MidpointRounding.AwayFromZero);
                mousePosition.Y = float.Round(mousePosition.Y, MidpointRounding.AwayFromZero);
            }

            if (!(SelectedChapter?.EnableShifting ?? false))
                goto ElementPlacement;

            Vector2 anchorPoint = movingAnchor ? chapterAnchor : SelectedChapter.ViewAnchor;
            anchorPoint = (anchorPoint - QuestAreaOffset) * Zoom;
            Rectangle anchor = CenteredRectangle(anchorPoint, new(14f));

            Vector2 cornerSize = new(32f);
            Point cornerOffset = (cornerSize * 0.5f).ToPoint();

            Vector2 minViewPoint = movingMinView ? chapterMinView : SelectedChapter.MinViewPoint;
            minViewPoint = (minViewPoint - QuestAreaOffset) * Zoom;
            Rectangle minView = CenteredRectangle(minViewPoint, cornerSize);
            minView.Offset(cornerOffset);

            Vector2 maxViewPoint = movingMaxView ? chapterMaxView : SelectedChapter.MaxViewPoint + defaultCanvasSize;
            maxViewPoint = (maxViewPoint - QuestAreaOffset) * Zoom;
            Rectangle maxView = CenteredRectangle(maxViewPoint, cornerSize);
            maxView.Offset(Point.Zero - cornerOffset);

            if (LeftMouseJustReleased && moveBounds)
            {
                if (questAreaTarget.Bounds.Contains(unsnapped.ToPoint()))
                {
                    if (movingMinView)
                    {
                        SelectedChapter.MinViewPoint = chapterMinView;
                        SelectedChapter.MinViewPoint = new(
                            float.Min(SelectedChapter.MinViewPoint.X, SelectedChapter.MaxViewPoint.X),
                            float.Min(SelectedChapter.MinViewPoint.Y, SelectedChapter.MaxViewPoint.Y));

                        chapterMinView = Vector2.Zero;
                    }

                    else if (movingMaxView)
                    {
                        SelectedChapter.MaxViewPoint = chapterMaxView - defaultCanvasSize;
                        SelectedChapter.MaxViewPoint = new(
                            float.Max(SelectedChapter.MaxViewPoint.X, SelectedChapter.MinViewPoint.X),
                            float.Max(SelectedChapter.MaxViewPoint.Y, SelectedChapter.MinViewPoint.Y));

                        chapterMaxView = Vector2.Zero;
                    }

                    else if (movingAnchor)
                    {
                        SelectedChapter.ViewAnchor = chapterAnchor;
                        chapterAnchor = Vector2.Zero;
                    }
                }

                movingMinView = !movingMinView && minView.Contains(unsnapped.ToPoint()) && placingElement is null && SelectedElement is null;
                movingMaxView = !movingMaxView && maxView.Contains(unsnapped.ToPoint()) && placingElement is null && SelectedElement is null;
                movingAnchor = !movingAnchor && anchor.Contains(unsnapped.ToPoint()) && placingElement is null && SelectedElement is null;
            }

            Vector2 left = minView.Location.ToVector2();
            Vector2 right = maxView.BottomRight();

            Vector2 angle = right - left;
            Vector2 center = left + (angle * 0.5f);

            center.Round();
            bool anchorCentered = false;

            if (moveBounds)
            {
                if (movingMinView)
                {
                    chapterMinView = mousePosition;
                    chapterMinView.Round();

                    if (RightMouseJustReleased)
                    {
                        SelectedChapter.MinViewPoint = Vector2.Zero;
                        chapterMinView = Vector2.Zero;
                        movingMinView = false;
                    }
                }

                else if (movingMaxView)
                {
                    chapterMaxView = mousePosition;
                    chapterMaxView.Round();

                    if (RightMouseJustReleased)
                    {
                        SelectedChapter.MaxViewPoint = Vector2.Zero;
                        chapterMaxView = Vector2.Zero;
                        movingMaxView = false;
                    }
                }

                else if (movingAnchor)
                {
                    chapterAnchor = mousePosition;

                    if (CenteredRectangle(center, new(28f)).Contains(unsnapped.ToPoint()))
                    {
                        chapterAnchor = center / Zoom + QuestAreaOffset;
                        chapterAnchor.Round();
                        anchorCentered = true;
                    }

                    if (RightMouseJustReleased)
                    {
                        SelectedChapter.ViewAnchor = defaultAnchor;
                        chapterAnchor = Vector2.Zero;
                        movingAnchor = false;
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.Draw(QuestAssets.BigPixel, center, null, Color.DarkGray, angle.ToRotation(), Vector2.One, new Vector2(angle.Length() * 0.5f, 1f), SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.CanvasCenter, center, null, Color.Black, 0f, QuestAssets.CanvasCenter.Asset.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

                    if (anchorCentered)
                        sb.Draw(QuestAssets.CanvasCenter, center, null, movingAnchor ? Color.Red : Color.Yellow, 0f, QuestAssets.CanvasCenter.Asset.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

                    else
                        sb.Draw(QuestAssets.CanvasCenter, anchor.Location.ToVector2(), movingAnchor ? Color.Red : Color.Yellow);

                    sb.Draw(QuestAssets.CanvasCorner, left, null, movingMinView ? Color.Red : Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.CanvasCorner, right, null, movingMaxView ? Color.Red : Color.Yellow, MathHelper.Pi, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                });
            }

            else if (showMidpoint)
            {
                Vector2 drawPos = new(float.Round(center.X), float.Round(center.Y));
                DrawTasks.Add(sb => sb.Draw(QuestAssets.CanvasCenter, drawPos, null, Color.LightGray, 0f, QuestAssets.CanvasCenter.Asset.Size() * 0.5f, 1f, SpriteEffects.None, 0f));
            }

        ElementPlacement:

            if (placingElement is null)
                return;

            if (LeftMouseJustReleased && mouseInBounds)
            {
                if (placingElement.PlaceOnCanvas(SelectedChapter, mousePosition, QuestAreaOffset))
                {
                    SelectedChapter.Elements.Add(placingElement);
                    SortedElements = null;
                    placingElement = null;
                }
            }

            DrawTasks.Add(sb => placingElement?.DrawPlacementPreview(sb, mousePosition, QuestAreaOffset, Zoom));
        }
    }
}
