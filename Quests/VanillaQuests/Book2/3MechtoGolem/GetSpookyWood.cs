using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2._3MechtoGolem
{
    internal class GetSpookyWood : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // collect 100 spooky wood from the harvest moon
                          // this is the first of the harvest moon branch, since frost moon already has 3 minibosses
                          // and summoner didnt have a "material nudge" quest, this fills both of those missing spots at once
        }
    }
}
