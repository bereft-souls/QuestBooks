using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class UseBinoculars : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for holding Binoculars to see the furthest you can
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
