using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Pickaxe
{
    internal class GetOrePickaxe : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // any copper/tin, iron/lead, tungstend/silver, gold/platinum pickaxe
        }
    }
}
