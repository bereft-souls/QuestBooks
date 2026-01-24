using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2.Endgame
{
    internal class EncounterCultists : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Get close to the Ancient Seal in the dungeon..
                         // ..orrr maybe this should just become an Info Quest? telling players to check the dungeon entrance?
        }
    }
}
