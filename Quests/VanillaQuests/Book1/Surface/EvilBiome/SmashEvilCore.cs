using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.EvilBiome
{
    internal class SmashEvilCore : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Break a shadow orb or crimson heart, achievement mirror
        }
    }
}
