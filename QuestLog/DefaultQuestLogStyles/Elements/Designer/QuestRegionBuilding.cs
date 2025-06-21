using Microsoft.Xna.Framework;
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

        public void DesignerPreQuestRegion()
        {
            if (ShowBackdrop)
                DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.15f));

            if (ShowGrid)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, LibraryBlending);
                });

                float linePosition = 0f;
                float gridSpacing = GridSize * targetScale;
                Color gridColor = Color.White with { A = 180 };

                while (linePosition <= questAreaTarget.Width)
                {
                    Rectangle gridLine = CenteredRectangle(new(linePosition, questAreaTarget.Height / 2), new(2f, questAreaTarget.Height));
                    AddRectangle(gridLine, gridColor, fill: true);
                    linePosition += gridSpacing;
                }

                linePosition = 0;
                while (linePosition <= questAreaTarget.Height)
                {
                    Rectangle gridLine = CenteredRectangle(new(questAreaTarget.Width / 2, linePosition), new(questAreaTarget.Width, 2f));
                    AddRectangle(gridLine, gridColor, fill: true);
                    linePosition += gridSpacing;
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, ContentBlending);
                });
            }
        }

        public void DesignerPostQuestRegion()
        {

        }
    }
}
