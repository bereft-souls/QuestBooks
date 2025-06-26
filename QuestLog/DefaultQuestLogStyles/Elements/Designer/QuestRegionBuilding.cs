using Microsoft.Xna.Framework;
using QuestBooks.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool ShowBackdrop = true;
        private static bool ShowGrid = false;
        private static bool SnapToGrid = false;
        private static int GridSize = 20;

        public void DesignerPreQuestRegion(Vector2 mousePosition)
        {
            if (SelectedChapter is null)
                return;

            if (ShowBackdrop)
                DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.15f));

            if (ShowGrid)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, GridBlending, sampler, depth, raster, effect, matrix);
                });

                Color gridColor = Color.White with { A = (byte)(ShowBackdrop ? 0 : 100) };

                for (int xPos = 0; xPos < 600; xPos += GridSize)
                {
                    Vector2 drawCenter = new(xPos, 300f);
                    Rectangle rect = CenteredRectangle(drawCenter, new(1f, 600f));
                    AddRectangle(rect, gridColor);
                }

                for (int yPos = 0; yPos < 600; yPos += GridSize)
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

        public void DesignerPostQuestRegion(Vector2 mousePosition, bool mouseInBounds)
        {
            if (placingElement is null)
                return;

            if (SnapToGrid)
            {
                mousePosition.X = float.Round(mousePosition.X / GridSize, MidpointRounding.AwayFromZero) * GridSize;
                mousePosition.Y = float.Round(mousePosition.Y / GridSize, MidpointRounding.AwayFromZero) * GridSize;
            }

            if (LeftMouseJustReleased && mouseInBounds)
            {
                if (placingElement.PlaceOnCanvas(SelectedChapter, mousePosition))
                {
                    SortedElements = null;
                    placingElement = null;
                }
            }

            DrawTasks.Add(sb => placingElement?.DrawPlacementPreview(sb, mousePosition));
        }
    }
}
