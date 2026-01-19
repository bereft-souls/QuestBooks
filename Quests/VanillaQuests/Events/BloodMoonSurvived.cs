using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    public class BloodMoonSurvived : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for Blood Moon "defeat"
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
