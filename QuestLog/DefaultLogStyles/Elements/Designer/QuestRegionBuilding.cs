using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuestBooks.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

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

        protected readonly Stack<(Action undo, Action redo)> ActionHistory = [];
        protected readonly Stack<(Action undo, Action redo)> RedoActions = [];
        protected bool HistoryAccessed { get; set; } = false; // Prevents repeated history access on consecutive frames

        protected void AddHistory(Action undo, Action redo)
        {
            ActionHistory.Push((undo, redo));
            RedoActions.Clear();
        }

        protected static Dictionary<MemberInfo, object> GetMemberwiseValues(ChapterElement element)
        {
            Dictionary<MemberInfo, object> result = [];

            foreach (var property in element.GetType().GetProperties().Where(p => p.CanWrite && p.CanRead && !p.PropertyType.IsByRef))
                result.Add(property, property.GetValue(element));

            foreach (var field in element.GetType().GetFields().Where(f => !f.FieldType.IsByRef))
                result.Add(field, field.GetValue(element));

            return result;
        }

        protected static void ApplyMemberwiseValues(ChapterElement element,  Dictionary<MemberInfo, object> values)
        {
            foreach (var member in values.Keys)
            {
                if (member is PropertyInfo property)
                    property.SetValue(element, values[member]);

                else
                    (member as FieldInfo).SetValue(element, values[member]);
            }
        }

        private void DesignerPreQuestRegion(Vector2 mousePosition)
        {
            // Action history (undo/redo)
            if (Main.keyState.PressingControl())
            {
                if (Main.keyState.IsKeyDown(Keys.Z) && // ctrl+Z
                    !Main.keyState.PressingShift())
                {
                    if (!HistoryAccessed && ActionHistory.TryPop(out var action))
                    {
                        action.undo();
                        RedoActions.Push(action);
                        HistoryAccessed = true;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }

                else if (Main.keyState.IsKeyDown(Keys.Y) || // ctrl+Y
                    (Main.keyState.PressingShift() && // ctrl+shift+Z
                    Main.keyState.IsKeyDown(Keys.Z)))
                {
                    if (!HistoryAccessed && RedoActions.TryPop(out var action))
                    {
                        action.redo();
                        ActionHistory.Push(action);
                        HistoryAccessed = true;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }

                else
                    HistoryAccessed = false;
            }

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
                        var chapter = SelectedChapter;
                        Vector2 oldMinView = chapter.MinViewPoint;

                        chapter.MinViewPoint = chapterMinView;
                        chapter.MinViewPoint = new(
                            float.Min(chapter.MinViewPoint.X, chapter.MaxViewPoint.X),
                            float.Min(chapter.MinViewPoint.Y, chapter.MaxViewPoint.Y));

                        chapterMinView = Vector2.Zero;
                        Vector2 newMinView = chapter.MinViewPoint;

                        if (oldMinView != newMinView)
                            AddHistory(() => {
                                chapter.MinViewPoint = oldMinView;
                            }, () => {
                                chapter.MinViewPoint = newMinView;
                            });
                    }

                    else if (movingMaxView)
                    {
                        var chapter = SelectedChapter;
                        Vector2 oldMaxView = chapter.MaxViewPoint;

                        chapter.MaxViewPoint = chapterMaxView - defaultCanvasSize;
                        chapter.MaxViewPoint = new(
                            float.Max(chapter.MaxViewPoint.X, chapter.MinViewPoint.X),
                            float.Max(chapter.MaxViewPoint.Y, chapter.MinViewPoint.Y));

                        chapterMaxView = Vector2.Zero;
                        Vector2 newMaxView = chapter.MaxViewPoint;

                        if (oldMaxView != newMaxView)
                            AddHistory(() => {
                                chapter.MaxViewPoint = oldMaxView;
                            }, () => {
                                chapter.MaxViewPoint = newMaxView;
                            });
                    }

                    else if (movingAnchor)
                    {
                        var chapter = SelectedChapter;
                        Vector2 oldAnchor = chapter.ViewAnchor;

                        chapter.ViewAnchor = chapterAnchor;
                        chapterAnchor = Vector2.Zero;
                        Vector2 newAnchor = chapter.ViewAnchor;

                        if (oldAnchor != newAnchor)
                            AddHistory(() => {
                                chapter.ViewAnchor = oldAnchor;
                            }, () => {
                                chapter.ViewAnchor = newAnchor;
                            });
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
                        var chapter = SelectedChapter;
                        Vector2 oldMinView = chapterMinView;

                        chapter.MinViewPoint = Vector2.Zero;
                        chapterMinView = Vector2.Zero;
                        movingMinView = false;

                        if (oldMinView != Vector2.Zero)
                            AddHistory(() => {
                                chapter.MinViewPoint = oldMinView;
                            }, () => {
                                chapter.MinViewPoint = Vector2.Zero;
                            });
                    }
                }

                else if (movingMaxView)
                {
                    chapterMaxView = mousePosition;
                    chapterMaxView.Round();

                    if (RightMouseJustReleased)
                    {
                        var chapter = SelectedChapter;
                        Vector2 oldMaxView = chapter.MaxViewPoint;

                        chapter.MaxViewPoint = Vector2.Zero;
                        chapterMaxView = Vector2.Zero;
                        movingMaxView = false;

                        if (oldMaxView != Vector2.Zero)
                            AddHistory(() => {
                                chapter.MaxViewPoint = oldMaxView;
                            }, () => {
                                chapter.MaxViewPoint = Vector2.Zero;
                            });
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
                        var chapter = SelectedChapter;
                        Vector2 oldAnchor = chapter.ViewAnchor;

                        chapter.ViewAnchor = defaultAnchor;
                        chapterAnchor = Vector2.Zero;
                        movingAnchor = false;

                        if (oldAnchor != defaultAnchor)
                            AddHistory(() => {
                                chapter.ViewAnchor = oldAnchor;
                            }, () => {
                                chapter.ViewAnchor = defaultAnchor;
                            });
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
                var oldMemberwiseValues = GetMemberwiseValues(placingElement);

                if (placingElement.PlaceOnCanvas(SelectedChapter, mousePosition, QuestAreaOffset))
                {
                    var chapter = SelectedChapter;
                    var element = placingElement;

                    chapter.Elements.Add(element);
                    SortedElements = null;
                    placingElement = null;

                    if (!element.PreviouslyPlaced)
                        AddHistory(() =>
                        {
                            chapter.Elements.Remove(element);
                            SortedElements = null;
                        }, () =>
                        {
                            chapter.Elements.Add(element);
                            SortedElements = null;
                        });

                    else
                    {
                        var newMemberwiseValues = GetMemberwiseValues(element);
                        AddHistory(() => {
                            ApplyMemberwiseValues(element, oldMemberwiseValues);
                        }, () => {
                            ApplyMemberwiseValues(element, newMemberwiseValues);
                        });
                    }

                    element.PreviouslyPlaced = true;
                }
            }

            DrawTasks.Add(sb => placingElement?.DrawPlacementPreview(sb, mousePosition, QuestAreaOffset, Zoom));
        }
    }
}
