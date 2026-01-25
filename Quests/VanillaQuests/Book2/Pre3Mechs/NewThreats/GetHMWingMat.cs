using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book2.Pre3Mechs.NewThreats
{
    internal class GetHMWingMat : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Get any wing-material (the ones used along with 20 souls of flight
            /* - NOTICE -
             * thought a bit about just, merging this and the wyvern quest into a multi-step quest revolving around making any wings? 
             * would still be a quest connected to the New Threats branch but would also just be a link to the Wing Quest in book 3's player progress
             */
        }
    }
}
