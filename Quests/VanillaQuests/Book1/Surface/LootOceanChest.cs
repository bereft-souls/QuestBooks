using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class LootOceanChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* TODO: check for any of:
                # Trident
                # Water Walking Boots
                # Breathing Reed
                # Flippers
                # Inner Tube
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
