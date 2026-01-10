using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    public class AllCelestialPillarDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return (NPC.downedTowerSolar && NPC.downedTowerVortex && NPC.downedTowerNebula && NPC.downedTowerStardust); //i saw theres a .downedTowers but the doc
                                                                                                                       //confused me on wether its just a bool or not
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "";     // TODO: Desc
            contents = "";
            texture = null;
        }
    }
}
