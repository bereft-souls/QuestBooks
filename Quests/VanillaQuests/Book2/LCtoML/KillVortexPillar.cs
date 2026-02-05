using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book2.Endgame
{
    internal class KillVortexPillar : Quest
    {
        public override bool CheckCompletion()
        {
            return NPC.downedTowerVortex;
        }
    }
}
