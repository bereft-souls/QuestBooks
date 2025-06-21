using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
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
        }
    }
}
