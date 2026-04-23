using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem
{
    internal class GetDungeonDrop : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // get any new dungeon drop weapon,
                          // INCLUDING shadow jousting lance (which the achievement forgets, as it was added afterwards
        }
    }
}
