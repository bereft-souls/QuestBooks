using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Hell
{
    internal class GetAshwood : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Enter an Ashwood Forest biome and chop an Ashwood Tree
        }
    }
}
