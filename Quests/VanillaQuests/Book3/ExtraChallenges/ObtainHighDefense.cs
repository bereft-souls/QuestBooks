using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.ExtraChallenges
{
    internal class ObtainHighDefense : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* Amass over roughly 100 Defense at once. 
             * this is slightly higher than post Ml Melee class without every single buff; 
             * but it does not require niche things like Dryad or stacking 3 paladin shields
             * so somewhere along that 100 line should be good.
             */
        }
    }
}
