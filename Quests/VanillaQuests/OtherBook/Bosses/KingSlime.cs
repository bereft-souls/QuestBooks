using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class KingSlime : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedSlimeKing;
    }
}
