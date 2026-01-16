using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class SturdyFossils : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for:
                          // 1) Get an Extractinator
                          // 2) Extract 15 Sturdy Fossils
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
