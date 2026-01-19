using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class InventoryInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Info node
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Your Inventory";
            contents = $""; // TODO: Write this later
            texture = null;
        }
    }
}
