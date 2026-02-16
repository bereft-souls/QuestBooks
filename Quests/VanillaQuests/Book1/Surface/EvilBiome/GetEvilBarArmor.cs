using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.EvilBiome
{
    internal class GetEvilBarArmor : DynamicQuest
    {
        // TODO, this entire thing
        // i ASSUME it would need to be a dynamic quest
        public override Asset<Texture2D> Texture => throw new NotImplementedException();

        public override bool CheckCompletion()
        {
            return false;
        }
    }
}
