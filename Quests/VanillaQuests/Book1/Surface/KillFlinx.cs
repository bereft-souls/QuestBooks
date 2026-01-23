using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class KillFlinx : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: check for:
                          // 1) get 5~ flinx fur
                          // 2) craft anything with it
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
