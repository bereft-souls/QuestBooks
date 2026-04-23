using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Caverns
{
    internal class ShimmerPhase : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Phase through 200~ blocks at once using shimmer phasing (shimmer liquid debuff)
            // in retrospect this is kindof more of an extra challenge than a main tree thing, maybe can be moved there
        }
    }
}
