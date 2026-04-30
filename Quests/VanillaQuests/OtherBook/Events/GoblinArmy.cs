using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events
{
    public class GoblinArmy : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedGoblins;
    }
}
