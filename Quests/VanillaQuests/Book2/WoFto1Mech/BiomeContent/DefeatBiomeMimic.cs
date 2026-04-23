using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2.Pre3Mechs.BiomeContent
{
    internal class DefeatBiomeMimic : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // any of Hallowed, Crimson or Corrupt mimic.
                          // include Jungle? even though its not normally spawned
        }
    }
}
