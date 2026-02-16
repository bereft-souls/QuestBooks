using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.ExtraChallenges
{
    internal class GetEveryTrophy : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // including event bosses
        }
    }
}
