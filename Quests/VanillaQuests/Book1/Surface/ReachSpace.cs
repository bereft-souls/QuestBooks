using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class ReachSpace : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for reaching space (top of the world), achievement mirror
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
