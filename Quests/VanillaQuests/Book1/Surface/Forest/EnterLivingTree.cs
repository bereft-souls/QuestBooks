using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.Forest
{
    internal class EnterLivingTree : Quest
    {
        public override bool CheckCompletion()
        {
            return false;  // TODO: Check for entering a living tree.. somehow
                          // Otherwise, check for opening a living wood chest instead
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
