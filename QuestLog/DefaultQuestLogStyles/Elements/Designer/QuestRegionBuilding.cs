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
        private static bool ShowBackdrop = false;
        private static bool ShowGrid = false;
        private static bool SnapToGrid = false;
        private static int GridSize = 19;

        public void DesignerPreQuestRegion()
        {
            if (ShowBackdrop)
                DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.12f));

            if (ShowGrid)
            {
                int linePosition = 0;
                Color gridColor = Color.White with { A = 175 };

                while (linePosition <= questAreaTarget.Width)
                {
                    Rectangle gridLine = CenteredRectangle(new(linePosition, questAreaTarget.Height / 2), new(2f, questAreaTarget.Height));
                    AddRectangle(gridLine, gridColor, fill: true);
                    linePosition += GridSize;
                }

                linePosition = 0;
                while (linePosition <= questAreaTarget.Height)
                {
                    Rectangle gridLine = CenteredRectangle(new(questAreaTarget.Width / 2, linePosition), new(questAreaTarget.Width, 2f));
                    AddRectangle(gridLine, gridColor, fill: true);
                    linePosition += GridSize;
                }
            }
        }

        public void DesignerPostQuestRegion()
        {

        }
    }
}
