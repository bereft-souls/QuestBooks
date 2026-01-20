using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class CrumbleDungeonBricks : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for breaking away some crumble bricks
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
