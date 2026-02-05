using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book3.MajorCraftingTrees.TerrasparkBoots
{
    internal class GetAnyRunningAcc : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* Get any running accessory, being:
             * Hermes Boots
             * Flurrystorm Boots
             * Dunerider Boots
             * Sailfish Boots
             */
        }
    }
}
