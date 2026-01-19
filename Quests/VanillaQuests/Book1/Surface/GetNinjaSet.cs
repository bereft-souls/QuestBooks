using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class GetNinjaSet : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for full ninja equipped + using shuriken
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
