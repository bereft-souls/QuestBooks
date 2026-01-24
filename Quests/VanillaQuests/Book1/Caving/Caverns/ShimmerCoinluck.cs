using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Caverns
{
    internal class ShimmerCoinluck : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Maximize coin luck by throwing the equivalency of 249,000 Copper Coins worth into the shimmer
                         // 1 Plat also instantly fulfills this
        }
    }
}
