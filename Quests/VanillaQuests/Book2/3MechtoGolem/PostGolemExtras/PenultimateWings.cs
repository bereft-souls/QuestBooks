using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem.PostGolemExtras
{
    internal class PenultimateWings : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Get wings from either Fishron or Empress of Light.......
                          // OOOOOOOOORRRRRRR, theoretically Betsy's wings should also be a part of this? theyre the same tier as bosses
                          // and as wingstats, a quest link to this could be made
                          // should probably discuss this a bit when time comes
        }
    }
}
