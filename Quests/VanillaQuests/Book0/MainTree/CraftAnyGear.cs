using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class CraftAnyGear : Quest
    {
        public override bool CheckCompletion()
        {
            return false;  // TODO: Needs to check for crafting any weapon or armor.
                          // Probably best to simply check when the player crafts any item with a damage or defense value?
        }
        
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Preparing for Trouble";
            contents = $"Craft yourself a piece of equipment to better your chances in combat!\r\n" +
                $"Any Broadsword, Bow or armorpiece should do.";
            texture = null;
        }
    }
}
