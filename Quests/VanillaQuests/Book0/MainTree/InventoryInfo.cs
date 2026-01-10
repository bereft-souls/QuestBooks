using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class InventoryInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Info node
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Your Inventory";
            contents = $""; // TODO: Write this later
            texture = null;
        }
    }
}
