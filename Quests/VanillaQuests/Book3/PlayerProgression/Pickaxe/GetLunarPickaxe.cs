using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Pickaxe
{
    internal class GetLunarPickaxe : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // get a pickaxe or a drill from any lunar fragment set
        }
    }
}
