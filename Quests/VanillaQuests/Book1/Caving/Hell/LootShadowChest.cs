using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Hell
{
    internal class LootShadowChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Get any 1 of:
            /* Sunfury
             * Flower of Fire
             * Flamelash
             * Dark Lance
             * Hellwing bow
             * */
        }
    }
}
