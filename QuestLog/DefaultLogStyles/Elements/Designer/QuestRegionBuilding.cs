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
        private static readonly Vector2 defaultCanvasSize = new(450, 530);

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
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, depth, raster, effect, matrix);
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

            Vector2 minViewPoint = movingMinView ? chapterMinView : SelectedChapter.MinViewPoint;
            minViewPoint -= QuestAreaOffset;
            Rectangle minView = CenteredRectangle(minViewPoint, new(32f));
            minView.Offset(24, 24);

            Vector2 maxViewPoint = movingMaxView ? chapterMaxView : SelectedChapter.MaxViewPoint + defaultCanvasSize;
            maxViewPoint -= QuestAreaOffset;
            Rectangle maxView = CenteredRectangle(maxViewPoint, new(32f));
            maxView.Offset(-16, -16);

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

            Vector2 cornerSize = new(32f);

            Rectangle leftCorner = CenteredRectangle(minViewPoint + new Vector2(10f), cornerSize);
            leftCorner.Offset((cornerSize * 0.5f).ToPoint());

            Rectangle rightCorner = CenteredRectangle(maxViewPoint, cornerSize);
            rightCorner.Offset((-cornerSize * 0.5f).ToPoint());

            Vector2 left = leftCorner.Location.ToVector2();
            Vector2 right = rightCorner.BottomRight();

            Vector2 angle = right - left;
            Vector2 center = left + (angle * 0.5f);

            if (moveBounds)
            {
                if (movingMinView)
                {
                    chapterMinView = mousePosition - new Vector2(16f);

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
                    sb.Draw(QuestAssets.CanvasCorner, left, null, movingMinView ? Color.Red : Color.Yellow, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
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
