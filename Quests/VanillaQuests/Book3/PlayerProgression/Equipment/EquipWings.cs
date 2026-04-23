using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Equipment
{
    internal class EquipWings : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // achievement mirror
            // while fledgeling wings exist, maybe this quest should be hidden until HM? 
        }
    }
}
