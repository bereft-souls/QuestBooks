using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using QuestBooks.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private readonly List<(Rectangle box, Type type)> elementSelections = [];
        private int elementTypeScrollOffset = 0;
        private ChapterElement placingElement = null;

        private void HandleQuestRegionTools()
        {
            Rectangle enableShifting = LogArea.CookieCutter(new(0.12f, -1.1f), new(0.069f, 0.075f));
            Rectangle moveBounds = enableShifting.CookieCutter(new(0f, -2.3f), Vector2.One);
            Rectangle showMidpoint = moveBounds.CookieCutter(new(2.2f, 0f), Vector2.One);
            Rectangle showBackdrop = enableShifting.CookieCutter(new(2.2f, 0f), Vector2.One);
            Rectangle showGrid = showBackdrop.CookieCutter(new(2.2f, 0f), Vector2.One);
            Rectangle snapGrid = showGrid.CookieCutter(new(2.2f, 0f), Vector2.One);
            float scale = enableShifting.Width / (float)QuestAssets.ShiftingCanvas.Asset.Width;

            Rectangle gridSize = snapGrid.CookieCutter(new(2.2f, 0f), new(0.95f, 1f));
            Rectangle gridUp = gridSize.CookieCutter(new(1.4f, -0.5f), new(0.4f, 0.5f));
            Rectangle gridDown = gridUp.CookieCutter(new(0f, 2f), Vector2.One);

            bool enableShiftingHovered = false;
            bool moveBoundsHovered = false;
            bool showMidpointHovered = false;
            bool showBackdropHovered = false;
            bool showGridHovered = false;
            bool snapGridHovered = false;

            bool gridSizeHovered = false;
            bool gridUpHovered = false;
            bool gridDownHovered = false;

            if (enableShifting.Contains(MouseCanvas))
            {
                LockMouse();
                enableShiftingHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShiftingCanvas");

                if (LeftMouseJustReleased && SelectedChapter is not null)
                {
                    SelectedChapter.EnableShifting = !SelectedChapter.EnableShifting;

                    if (SelectedChapter.EnableShifting)
                        SelectedChapter.ViewAnchor = defaultAnchor;

                    else
                        QuestAreaOffset = Vector2.Zero;
                }
            }

            if (SelectedChapter is not null)
            {
                if (moveBounds.Contains(MouseCanvas))
                {
                    LockMouse();
                    moveBoundsHovered = true;
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.MoveBounds");

                    if (LeftMouseJustReleased)
                        this.moveBounds = !this.moveBounds;
                }

                else if (showMidpoint.Contains(MouseCanvas))
                {
                    LockMouse();
                    showMidpointHovered = true;
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShowMidpoint");

                    if (LeftMouseJustReleased)
                        this.showMidpoint = !this.showMidpoint;
                }
            }

            if (showBackdrop.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ToggleBackdrop");
                showBackdropHovered = true;

                if (LeftMouseJustReleased)
                    this.showBackdrop = !this.showBackdrop;
            }

            if (showGrid.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ToggleGrid");
                showGridHovered = true;

                if (LeftMouseJustReleased)
                    this.showGrid = !this.showGrid;
            }

            if (snapGrid.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SnapGrid");
                snapGridHovered = true;

                if (LeftMouseJustReleased)
                    snapToGrid = !snapToGrid;
            }

            if (gridSize.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.GridSize");
                gridSizeHovered = true;

                if (LeftMouseJustReleased)
                    this.gridSize = 20;
            }

            else if (gridUp.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.GridSizeUp");
                gridUpHovered = true;

                if (LeftMouseJustReleased)
                    this.gridSize++;
            }

            else if (gridDown.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.GridSizeDown");
                gridDownHovered = true;

                if (LeftMouseJustReleased && this.gridSize > 3)
                    this.gridSize--;
            }

            DrawTasks.Add(sb =>
            {
                void DrawToggle(Rectangle area, bool hovered, Texture2D button, Texture2D buttonHovered, float opacity = 1f, bool outline = false)
                {
                    Texture2D texture = hovered ? buttonHovered : button;
                    Vector2 center = area.Center();

                    if (outline)
                        sb.Draw(QuestAssets.ToolOutline, center, null, Color.Yellow * opacity, 0f, QuestAssets.ToolOutline.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                    sb.Draw(texture, center, null, Color.White * opacity, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                }

                bool active = SelectedChapter is not null;
                DrawToggle(enableShifting, enableShiftingHovered, QuestAssets.ShiftingCanvas, active ? QuestAssets.ShiftingCanvasHovered : QuestAssets.ShiftingCanvas, active ? 1f : 0.5f, SelectedChapter?.EnableShifting ?? false);

                if (this.showBackdrop)
                    DrawToggle(showBackdrop, showBackdropHovered, QuestAssets.ToggleBackdropEnabled, QuestAssets.ToggleBackdropEnabledHovered, outline: true);

                else
                    DrawToggle(showBackdrop, showBackdropHovered, QuestAssets.ToggleBackdropDisabled, QuestAssets.ToggleBackdropDisabledHovered);

                DrawToggle(showGrid, showGridHovered, QuestAssets.ToggleGrid, QuestAssets.ToggleGridHovered, outline: this.showGrid);
                DrawToggle(snapGrid, snapGridHovered, QuestAssets.GridSnapping, QuestAssets.GridSnappingHovered, outline: this.snapToGrid);

                DrawToggle(gridSize, gridSizeHovered, QuestAssets.GridSize, QuestAssets.GridSizeHovered);
                DrawToggle(gridUp, gridUpHovered, QuestAssets.GridSizeUp, QuestAssets.GridSizeUpHovered);
                DrawToggle(gridDown, gridDownHovered, QuestAssets.GridSizeDown, QuestAssets.GridSizeDownHovered);

                Rectangle gridText = gridSize.CookieCutter(new(0.5f, 0.2f), new(0.75f, 0.75f));
                sb.DrawOutlinedStringInRectangle(gridText, FontAssets.DeathText.Value, Color.White, Color.Black, this.gridSize.ToString(), clipBounds: false);

                if (active && SelectedChapter.EnableShifting)
                {
                    DrawToggle(moveBounds, moveBoundsHovered, QuestAssets.MoveBounds, QuestAssets.MoveBoundsHovered, outline: this.moveBounds);
                    DrawToggle(showMidpoint, showMidpointHovered, QuestAssets.DisplayMidpoint, QuestAssets.DisplayMidpointHovered, outline: this.showMidpoint);
                }
            });

            if (SelectedChapter is not null)
            {
                Rectangle elementTypeSelection = LogArea.CookieCutter(new(1.24f, 0f), new(0.23f, 0.9f));
                AddRectangle(elementTypeSelection, Color.Gray * 0.6f, fill: true);
                AddRectangle(elementTypeSelection, Color.Black, 3f);

                Rectangle elementTypeDisplay = elementTypeSelection.CookieCutter(new(0f, -1.03f), new(1f, 0.075f));
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(elementTypeDisplay, FontAssets.DeathText.Value, Color.White, Color.Black, "Element Selection:"));

                Rectangle typeBox = elementTypeSelection.CreateScaledMargin(0.025f).CookieCutter(new(0f, -0.95f), new(1f, 0.078f));
                elementSelections.Clear();

                foreach (Type elementType in QuestManager.AvailableQuestElementTypes.Keys)
                {
                    elementSelections.Add((typeBox, elementType));
                    typeBox = typeBox.CookieCutter(new(0, 2.2f), Vector2.One);
                }

                if (elementTypeSelection.Contains(MouseCanvas))
                {
                    LockMouse();
                    int data = PlayerInput.ScrollWheelDeltaForUI;

                    if (data != 0)
                    {
                        int scrollAmount = data / 6;
                        int initialOffset = elementTypeScrollOffset;
                        elementTypeScrollOffset += scrollAmount;

                        Rectangle lastBox = elementSelections[^1].box;
                        int minScrollValue = -(lastBox.Bottom - (elementTypeSelection.Height + elementTypeSelection.Y));

                        elementTypeScrollOffset = minScrollValue < 0 ? int.Clamp(elementTypeScrollOffset, minScrollValue, 0) : 0;
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                    sb.GraphicsDevice.ScissorRectangle = elementTypeSelection;
                    raster.ScissorTestEnable = true;

                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                });

                foreach (var (box, elementType) in elementSelections)
                {
                    bool placing = (placingElement?.GetType() ?? null) == elementType;
                    bool otherPlacing = !placing && placingElement is not null;

                    box.Offset(0, elementTypeScrollOffset);

                    if (!placing)
                    {
                        AddRectangle(box, Color.Gray, fill: true);
                        AddRectangle(box, Color.LightGray);
                    }

                    else
                    {
                        AddRectangle(box, Color.PaleGoldenrod, fill: true);
                        AddRectangle(box, Color.Yellow);
                    }

                    Rectangle textArea = box.CookieCutter(new(0.2f, 0f), new(0.78f, 1f));
                    Rectangle iconArea = box.CookieCutter(new(-0.775f, 0f), new(0.225f, 1f)).CreateScaledMargin(0.2f);

                    DrawTasks.Add(sb =>
                    {
                        sb.DrawOutlinedStringInRectangle(textArea.CookieCutter(new(0f, 0.25f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, elementType.Name);
                        QuestManager.AvailableQuestElementTypes[elementType].DrawDesignerIcon(sb, iconArea);
                    });

                    if (box.Contains(MouseCanvas) && elementTypeSelection.Contains(MouseCanvas))
                    {
                        if (!placing)
                            AddRectangle(box, Color.White);

                        MouseTooltip = $"[c/CCC018:{elementType.FullName}]";

                        if (Attribute.GetCustomAttribute(elementType, typeof(TooltipAttribute)) is TooltipAttribute tooltip)
                            MouseTooltip += $"\n{Language.GetTextValue(tooltip.LocalizationKey)}";

                        if (LeftMouseJustReleased)
                        {
                            if ((placingElement?.GetType() ?? null) != elementType)
                                placingElement = (ChapterElement)Activator.CreateInstance(elementType);

                            else
                                placingElement = null;
                        }
                    }
                }
            }

            if (RightMouseJustReleased)
                placingElement = null;
        }
    }
}
