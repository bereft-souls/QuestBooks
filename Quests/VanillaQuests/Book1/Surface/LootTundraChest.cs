using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
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
