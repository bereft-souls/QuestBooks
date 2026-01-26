using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem
{
    internal class LootTempleChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Open a temple chest
                          // this quest may sound redundant but its description has relevent info for solar eclipse 2
        }
    }
}
