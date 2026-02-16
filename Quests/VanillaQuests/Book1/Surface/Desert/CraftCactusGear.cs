using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.Desert
{
    internal class CraftCactusGear : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for:
                          // 1) Getting X Cactus,
                          // 2) Craft any gear
        }
    }
}
