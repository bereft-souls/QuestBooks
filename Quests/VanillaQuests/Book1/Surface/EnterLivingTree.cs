using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class EnterLivingTree : Quest
    {
        public override bool CheckCompletion()
        {
            return false;  // TODO: Check for entering a living tree.. somehow
                           // Otherwise, check for opening a living wood chest instead
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
