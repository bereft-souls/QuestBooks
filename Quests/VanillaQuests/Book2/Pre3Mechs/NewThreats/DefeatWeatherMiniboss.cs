using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2.Pre3Mechs.NewThreats
{
    internal class DefeatWeatherMiniboss : Quest
    {
        public override bool CheckCompletion()
        {
            return false; //Kill either an Ice Golem or a Sand Elemental
        }
    }
}
