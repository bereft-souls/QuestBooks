using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class GetEoCSummon : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for getting or crafting a Suspicious Looking Eye
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
