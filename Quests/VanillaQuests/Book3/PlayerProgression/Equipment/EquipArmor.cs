using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Equipment
{
    internal class EquipArmor : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // fill all 3 slots with an armor piece (not neccessarily the same set), achievement mirror
        }
    }
}
