using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class LootSurfaceChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* TODO: check for any of:
                - Spear
                - Blowpipe
                - Wooden Boomerang
                - Umbrella
                - Wand of Sparking
                - Aglet
                - Climbing Claws
                - Radar
                - Guide to Fiber Cordage
                - Stepstool
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
