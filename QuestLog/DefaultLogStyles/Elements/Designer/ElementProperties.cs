using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool preview = false;

        private void HandleElementProperties()
        {
            if (preview)
            {
                DrawTasks.Add(SelectedElement.DrawInfoPage);
                return;
            }


        }
    }
}
