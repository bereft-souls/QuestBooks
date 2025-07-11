using Microsoft.Xna.Framework;
using QuestBooks.Assets;
using System;
using Terraria;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool showBackdrop = false;
        private static bool showGrid = false;
        private static bool snapToGrid = false;
        private static int gridSize = 20;

        private static bool moveBounds = true;
        private static bool showMidpoint = true;
        private static bool movingAnchor = false;
        private static bool movingMaxView = false;

        private static Vector2 chapterAnchor = Vector2.Zero;
        private static Vector2 chapterMaxView = Vector2.Zero;

        private static readonly Vector2 defaultAnchor = new(230f, 270f);
        private static readonly Vector2 defaultMaxView = new(450, 530);

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
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, GridBlending, sampler, depth, raster, effect, matrix);
                });

                Color gridColor = Color.White with { A = (byte)(showBackdrop ? 0 : 100) };

                for (int xPos = 0; xPos < 500; xPos += gridSize)
                {
                    Vector2 drawCenter = new(xPos - (QuestAreaOffset.X % gridSize), 300f);
                    Rectangle rect = CenteredRectangle(drawCenter, new(1f, 600f));
                    AddRectangle(rect, gridColor);
                }

                for (int yPos = 0; yPos < 600; yPos += gridSize)
                {
                    Vector2 drawCenter = new(300f, yPos - (QuestAreaOffset.Y % gridSize));
                    Rectangle rect = CenteredRectangle(drawCenter, new(600f, 1f));
                    AddRectangle(rect, gridColor);
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, ContentBlending, sampler, depth, raster, effect, matrix);
                });
            }
        }

        private void DesignerPostQuestRegion(Vector2 mousePosition, bool mouseInBounds)
        {
            // Round to grid intersections
            Vector2 unsnapped = mousePosition - QuestAreaOffset;
            if (snapToGrid)
            {
                mousePosition.X = float.Round(mousePosition.X / gridSize, MidpointRounding.AwayFromZero) * gridSize;
                mousePosition.Y = float.Round(mousePosition.Y / gridSize, MidpointRounding.AwayFromZero) * gridSize;
            }

            if (!(SelectedChapter?.EnableShifting ?? false))
                goto ElementPlacement;

            Vector2 anchorPoint = movingAnchor ? chapterAnchor : SelectedChapter.ViewAnchor;
            anchorPoint -= QuestAreaOffset;
            Rectangle anchor = CenteredRectangle(anchorPoint, new(14f));

            Vector2 maxViewPoint = movingMaxView ? chapterMaxView : SelectedChapter.MaxViewPoint + defaultMaxView;
            maxViewPoint -= QuestAreaOffset;
            Rectangle maxView = CenteredRectangle(maxViewPoint, new(32f));
            maxView.Offset(-16, -16);

            if (LeftMouseJustReleased && moveBounds)
            {
                if (mouseInBounds)
                {
                    if (movingMaxView)
                    {
                        SelectedChapter.MaxViewPoint = chapterMaxView - defaultMaxView;
                        SelectedChapter.MaxViewPoint = new(float.Max(SelectedChapter.MaxViewPoint.X, 0f), float.Max(SelectedChapter.MaxViewPoint.Y, 0f));
                        chapterMaxView = Vector2.Zero;
                    }

                    else if (movingAnchor)
                    {
                        SelectedChapter.ViewAnchor = chapterAnchor;
                        chapterAnchor = Vector2.Zero;
                    }
                }

                movingMaxView = !movingMaxView && maxView.Contains(unsnapped.ToPoint()) && placingElement is null && SelectedElement is null;
                movingAnchor = !movingAnchor && anchor.Contains(unsnapped.ToPoint()) && placingElement is null && SelectedElement is null;
            }

            Vector2 cornerSize = new(32f);
            Vector2 topLeft = new Vector2(10f) - QuestAreaOffset;

            Rectangle leftCorner = CenteredRectangle(topLeft, cornerSize);
            leftCorner.Offset((cornerSize * 0.5f).ToPoint());

            Rectangle rightCorner = CenteredRectangle(maxViewPoint, cornerSize);
            rightCorner.Offset((-cornerSize * 0.5f).ToPoint());

            Vector2 left = leftCorner.Location.ToVector2();
            Vector2 right = rightCorner.BottomRight();

            Vector2 angle = right - left;
            Vector2 center = left + (angle * 0.5f);

            if (moveBounds)
            {
                if (movingMaxView)
                {
                    chapterMaxView = mousePosition;

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
                        chapterAnchor = center + QuestAreaOffset;

                    if (RightMouseJustReleased)
                    {
                        SelectedChapter.ViewAnchor = defaultAnchor;
                        chapterAnchor = Vector2.Zero;
                        movingAnchor = false;
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.Draw(QuestAssets.BigPixel, center, null, Color.DarkGray, angle.ToRotation(), Vector2.One, new Vector2(angle.Length() * 0.5f, 1f), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.CanvasCenter, center, null, Color.DarkGray, 0f, QuestAssets.CanvasCenter.Asset.Size() * 0.5f, 2f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);

                    sb.Draw(QuestAssets.CanvasCenter, anchor.Location.ToVector2(), movingAnchor ? Color.Red : Color.Yellow);
                    sb.Draw(QuestAssets.CanvasCorner, left, Color.Yellow);
                    sb.Draw(QuestAssets.CanvasCorner, right, null, movingMaxView ? Color.Red : Color.Yellow, MathHelper.Pi, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                });
            }

            else if (showMidpoint)
            {
                Vector2 drawPos = new(float.Round(center.X), float.Round(center.Y));
                DrawTasks.Add(sb => sb.Draw(QuestAssets.CanvasCenter, drawPos, null, Color.LightGray, 0f, QuestAssets.CanvasCenter.Asset.Size() * 0.5f, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f));
            }

        ElementPlacement:

            if (placingElement is null)
                return;

            if (LeftMouseJustReleased && mouseInBounds)
            {
                if (placingElement.PlaceOnCanvas(SelectedChapter, mousePosition))
                {
                    SelectedChapter.Elements.Add(placingElement);
                    SortedElements = null;
                    placingElement = null;
                }
            }

            DrawTasks.Add(sb => placingElement?.DrawPlacementPreview(sb, mousePosition, QuestAreaOffset));
        }
    }
}
