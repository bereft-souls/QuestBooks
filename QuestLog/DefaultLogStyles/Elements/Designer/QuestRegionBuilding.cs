using Microsoft.Xna.Framework;
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

                for (int xPos = 0; xPos < 600; xPos += gridSize)
                {
                    Vector2 drawCenter = new(xPos, 300f);
                    Rectangle rect = CenteredRectangle(drawCenter, new(1f, 600f));
                    AddRectangle(rect, gridColor);
                }

                for (int yPos = 0; yPos < 600; yPos += gridSize)
                {
                    Vector2 drawCenter = new(300f, yPos);
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
            if (placingElement is null)
                return;

            // Round to grid intersections
            if (snapToGrid)
            {
                mousePosition.X = float.Round(mousePosition.X / gridSize, MidpointRounding.AwayFromZero) * gridSize;
                mousePosition.Y = float.Round(mousePosition.Y / gridSize, MidpointRounding.AwayFromZero) * gridSize;
            }

            if (LeftMouseJustReleased && mouseInBounds)
            {
                if (placingElement.PlaceOnCanvas(SelectedChapter, mousePosition))
                {
                    SelectedChapter.Elements.Add(placingElement);
                    SortedElements = null;
                    placingElement = null;
                }
            }

            DrawTasks.Add(sb => placingElement?.DrawPlacementPreview(sb, mousePosition));
        }
    }
}
