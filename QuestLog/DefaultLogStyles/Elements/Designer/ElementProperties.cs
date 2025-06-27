using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool previewElementInfo = false;

        private void HandleElementProperties()
        {
            if (previewElementInfo)
            {
                DrawTasks.Add(SelectedElement.DrawInfoPage);
                return;
            }

            DrawTasks.Add(sb => sb.GraphicsDevice.Clear(Color.Blue * 0.5f));
        }
    }
}
