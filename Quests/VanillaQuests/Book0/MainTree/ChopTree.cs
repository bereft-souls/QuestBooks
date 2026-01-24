using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class ChopTree : Quest
    {
        public override bool CheckCompletion()
        {
            return false;  // TODO: checked when player chops tree, could piggyback off the existing achievement?
                          // couldnt figure out how to access player data yet lol
        }
    }
}
