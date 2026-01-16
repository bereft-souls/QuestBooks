using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class BiomeSky : Quest
    {
        public override bool CheckCompletion()
        {
            return false;   // TODO: Check for entering sky layer
                           // specifically, when Sky enemies start to spawn in PreHm / steampunker or painter start selling their space exclusive items.
                          // im mentioning this because apparently space is extremely gradual and not a clear biome transition.
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
