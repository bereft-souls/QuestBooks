using Microsoft.Xna.Framework;
using QuestBooks.Utilities;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.GameInput;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static readonly List<(Rectangle box, Type type)> ElementSelections = [];
        private static int ElementTypeScrollOffset = 0;
        private static ChapterElement placingElement = null;

        private static void HandleQuestRegionTools()
        {
            Rectangle enableShifting = LogArea.CookieCutter(new(0.15f, -1.1f), new(0.065f, 0.06f));
            Rectangle showBackdrop = enableShifting.CookieCutter(new(2.2f, 0f), Vector2.One);
            Rectangle showGrid = showBackdrop.CookieCutter(new(2.2f, 0f), Vector2.One);
            Rectangle snapGrid = showGrid.CookieCutter(new(2.2f, 0f), Vector2.One);

            Rectangle gridSize = snapGrid.CookieCutter(new(2f, 0f), new(0.8f, 1f));
            Rectangle gridUp = gridSize.CookieCutter(new(1.3f, -0.5f), new(0.3f, 0.5f));
            Rectangle gridDown = gridUp.CookieCutter(new(0f, 2f), Vector2.One);

            AddRectangle(enableShifting, SelectedChapter is null ? Color.Red * 0.4f : Color.Red, fill: true);
            AddRectangle(showBackdrop, Color.Lime, fill: true);
            AddRectangle(showGrid, Color.Blue, fill: true);
            AddRectangle(snapGrid, Color.Yellow, fill: true);

            AddRectangle(gridSize, Color.Magenta, fill: true);
            AddRectangle(gridUp, Color.Purple, fill: true);
            AddRectangle(gridDown, Color.Violet, fill: true);

            if (enableShifting.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Enable shifting canvas";

                if (LeftMouseJustReleased && SelectedChapter is not null)
                    SelectedChapter.EnableShifting = !SelectedChapter.EnableShifting;
            }

            if (SelectedChapter?.EnableShifting ?? false)
                AddRectangle(enableShifting, Color.White, 3f);

            if (showBackdrop.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Show backdrop";

                if (LeftMouseJustReleased)
                    ShowBackdrop = !ShowBackdrop;
            }

            if (ShowBackdrop)
                AddRectangle(showBackdrop, Color.White, 3f);

            if (showGrid.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Show grid";

                if (LeftMouseJustReleased)
                    ShowGrid = !ShowGrid;
            }

            if (ShowGrid)
                AddRectangle(showGrid, Color.White, 3f);

            if (snapGrid.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Snap to grid";

                if (LeftMouseJustReleased)
                    SnapToGrid = !SnapToGrid;
            }

            if (SnapToGrid)
                AddRectangle(snapGrid, Color.White, 3f);

            if (gridSize.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Grid size";
            }

            else if (gridUp.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Grid size up";

                if (LeftMouseJustReleased)
                    GridSize++;
            }

            else if (gridDown.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = "Grid size down";

                if (LeftMouseJustReleased && GridSize > 3)
                    GridSize--;
            }

            if (SelectedChapter is not null)
            {
                Rectangle elementTypeSelection = LogArea.CookieCutter(new(1.24f, 0f), new(0.23f, 0.9f));
                AddRectangle(elementTypeSelection, Color.Gray * 0.6f, fill: true);
                AddRectangle(elementTypeSelection, Color.Black, 3f);

                Rectangle elementTypeDisplay = elementTypeSelection.CookieCutter(new(0f, -1.03f), new(1f, 0.075f));
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(elementTypeDisplay, FontAssets.DeathText.Value, Color.White, Color.Black, "Element Selection:"));

                Rectangle typeBox = elementTypeSelection.CreateScaledMargin(0.025f).CookieCutter(new(0f, -0.95f), new(1f, 0.078f));
                ElementSelections.Clear();

                foreach (Type elementType in AvailableQuestElementTypes.Keys)
                {
                    ElementSelections.Add((typeBox, elementType));
                    typeBox = typeBox.CookieCutter(new(0, 2.2f), Vector2.One);
                }

                if (elementTypeSelection.Contains(MouseCanvas))
                {
                    LockMouse();
                    int data = PlayerInput.ScrollWheelDeltaForUI;

                    if (data != 0)
                    {
                        int scrollAmount = data / 6;
                        int initialOffset = ElementTypeScrollOffset;
                        ElementTypeScrollOffset += scrollAmount;

                        Rectangle lastBox = ElementSelections[^1].box;
                        int minScrollValue = -(lastBox.Bottom - (elementTypeSelection.Height + elementTypeSelection.Y));

                        ElementTypeScrollOffset = minScrollValue < 0 ? int.Clamp(ElementTypeScrollOffset, minScrollValue, 0) : 0;
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                    sb.GraphicsDevice.ScissorRectangle = elementTypeSelection;
                    raster.ScissorTestEnable = true;

                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                });

                foreach (var (box, elementType) in ElementSelections)
                {
                    bool placing = (placingElement?.GetType() ?? null) == elementType;
                    bool otherPlacing = !placing && placingElement is not null;

                    box.Offset(0, ElementTypeScrollOffset);

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
                        AvailableQuestElementTypes[elementType].DrawDesignerIcon(sb, iconArea);
                    });

                    if (box.Contains(MouseCanvas) && elementTypeSelection.Contains(MouseCanvas))
                    {
                        if (!placing)
                            AddRectangle(box, Color.White);

                        MouseTooltip = elementType.FullName;

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
