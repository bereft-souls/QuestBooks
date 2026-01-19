using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class LootDesertChest : Quest
    {
        public override bool CheckCompletion()
        {
            return false;
            /* TODO: check for any of:
                - Magic Conch
                - Snake Charmer's Flute
                - Ancient Chisel
                - Dunerider's Boots
                - Thunder Zapper
                - Storm Spear
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
