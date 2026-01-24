using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Caverns
{
    internal class LootWebChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // NOTICE: This is actually a "Get Web Slinger" quest, its simply guarenteed in all web chests
            // maybe the class name is misleading but its accurate to other chest quests, can be renamed though
        }
    }
}
