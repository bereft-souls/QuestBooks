using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Pickaxe
{
    internal class GetHMOrePickaxes : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // get a pickaxe or a drill from these 3 groups
                            // Cobalt / Palladium
                            // Mythril / Orichalcum
                            // Adamantite / Titanium
        }
    }
}
