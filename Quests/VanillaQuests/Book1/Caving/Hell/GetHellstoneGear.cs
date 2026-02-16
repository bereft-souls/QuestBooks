using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Hell
{
    internal class GetHellstoneGear : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Craft any item using Hellstone Bars
        }
    }
}
