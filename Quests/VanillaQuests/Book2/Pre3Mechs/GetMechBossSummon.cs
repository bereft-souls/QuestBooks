using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2.Pre3Mechs
{
    internal class GetMechBossSummon : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Either craft one of them, or get as a drop
        }
    }
}
