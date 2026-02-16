using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.Dungeon
{
    internal class BiomeDungeon : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for entering dungeon after skeletron's defeat (entering and dying to guardian shouldnt count id say
        }
    }
}
