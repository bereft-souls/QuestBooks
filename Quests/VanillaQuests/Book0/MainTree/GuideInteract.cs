using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class GuideInteract : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // Should be a check for interacting with the guide (right clicking him)
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "The Guide";
            contents = $"A trusty guide to help you along the way with their incredible crafting knowledge and varying useful tips.\n" +
                $"If you ever find an item labeled as a \"Material\", be sure to check it up with him!";
            texture = null;
        }
    }
}
