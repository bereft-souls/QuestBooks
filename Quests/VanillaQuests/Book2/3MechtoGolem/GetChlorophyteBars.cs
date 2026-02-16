using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem
{
    internal class GetChlorophyteBars : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // get X chlorophyte bars.
                          // X should be a decently sizable amount, as chlorophyte on its own has hefty usage
                          // shouldnt require crafting anything with it thise time,
                          // as theres 3+ quests connected to this one where Chlorophyte is a material anyways
        }
    }
}
