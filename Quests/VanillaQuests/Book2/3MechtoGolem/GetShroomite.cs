using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem
{
    internal class GetShroomite : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Craft a shroomite bar at an autocrafter
        }
    }
}
