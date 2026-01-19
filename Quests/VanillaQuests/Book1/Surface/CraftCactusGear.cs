using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class CraftCactusGear : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for:
                          // 1) Getting X Cactus,
                          // 2) Craft any gear
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
