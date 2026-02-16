using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    public class GoblinInvasionDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return NPC.downedGoblins;
        }
        
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Goblin Punter";
            contents = "Triumph over a goblin invasion, a ragtag regiment of crude, barbaric, pointy-eared warriors and their shadowflame sorcerers.";
            texture = null;
        }
    }
}
