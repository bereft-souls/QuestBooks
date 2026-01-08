using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0
{
    internal class ChopTree : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // checked when player chops tree, could piggyback off the existing achievement? couldnt figure out how to access player data yet lol
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Timber!";
            contents = $"Chop down a nearby tree using your trusted Copper Axe!\n" +
                $"Tip: holding LeftShift while hovering over any block will automatically swap you to the required tool.";
            texture = null;
        }
    }
}
