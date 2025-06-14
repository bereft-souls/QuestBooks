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
        // Handles the display of a selected questline element
        public static QuestLineElement SelectedElement = null;

        //private static Vector2 previousQuestAreaOffset = Vector2.Zero;

        private void UpdateQuestArea(Rectangle questArea)
        {
            SwitchTargets(questAreaTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.12f));
            SwitchTargets(null);
        }
    }
}
