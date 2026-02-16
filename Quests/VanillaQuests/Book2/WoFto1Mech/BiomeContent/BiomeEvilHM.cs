using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace QuestBooks.Quests.VanillaQuests.Book2.Pre3Mechs.BiomeContent
{
    internal class BiomeEvilHM : DynamicQuest
    {
        public override Asset<Texture2D> Texture => throw new NotImplementedException();

        public override bool CheckCompletion()
        {
            return false; // Enter an evil biome in HM, maybe caverns specifically as that usually doesnt exist before hm
        }
    }
}
