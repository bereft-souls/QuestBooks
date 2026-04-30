using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0
{
    public class StartFirstSteps : QBQuest
    {
        public override bool CheckCompletion()
        {
            return true; // Start of tree, always unlocked
        }
    }
}
