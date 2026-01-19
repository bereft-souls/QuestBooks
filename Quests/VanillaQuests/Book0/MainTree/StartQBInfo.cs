using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class StartQBInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true;
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Introduction to Questing";
            contents = ""; // TODO: add somewhat indepth explanation of how this works
            texture = null;
        }
    }
}
