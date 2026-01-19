using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class EquipEyeBone : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for getting & equiping Eye Bone (chester pet)
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
