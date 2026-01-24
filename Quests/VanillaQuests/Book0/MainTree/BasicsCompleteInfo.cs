using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class BasicsCompleteInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Info node
        }
    }
}
