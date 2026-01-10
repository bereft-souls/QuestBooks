using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class ForestBiome : Quest
    {
        public override bool CheckCompletion()
        {
            return false;  // TODO: Should be a check for being in the forest biome
                         // which would still be an insta-check 99% of the time (in non-special seed worlds) but yeah
        }
        
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "The Forest";
            contents = $"The starting location of all Terrarians." +
                $"\n" +
                $"Between the lush greenery and comfy breeze, it seems like the perfect spot to set up base in!";
            texture = null;
        }

    }
}
