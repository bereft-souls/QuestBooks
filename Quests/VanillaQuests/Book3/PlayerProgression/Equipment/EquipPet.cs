using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.PlayerProgression.Equipment
{
    internal class EquipPet : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // either normal or a light pet
        }
    }
}
