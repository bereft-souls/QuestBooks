using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.ExtraContentProgression.Fishing
{
    internal class GetAnglerSet : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // equip all 3 pieces of the angler set
        }
    }
}
