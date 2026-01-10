using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0
{
    public class CraftHammer : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for hammer craft / acquisation, achievement mirror
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Hammer time!";
            contents = $"Craft a Wooden Hammer.\r\n" +
                $"Hammers are used to break apart background wall tiles, and allows the reshaping of tiles by hitting them";
            texture = null;
        }
    }
}
