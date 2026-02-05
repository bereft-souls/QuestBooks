using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.ExtraChallenges
{
    internal class DefeatDayEmpress : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // can also check for getting Terraprisma? would suck for mods that edit that drop req though
        }
    }
}
