using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface.Tundra
{
    internal class LootTundraChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* TODO: check for any of:
                # Ice Boomerang
                # Ice Blade
                # Ice Skates
                # Snowball Cannon
                # Blizzard in a Bottle
                # Flurry Boots
                # Extractinator
                # Fish (pet item)
            */
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
