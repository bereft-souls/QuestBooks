using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class EstablishHouse : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for a valid house being constructed, achievement mirror
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "No Hobo";
            contents = $"Build a safe shelter for you and your Guide!\r\n" +
                $"\r\n" +
                $"In Terraria, Houses require specific conditions to become valid for tenants:\r\n" +
                $"- 60 blocks of space, covered with Background Walls.\r\n" +
                $"- A Chair and Table (your workbench works too!)\r\n" +
                $"- Any type of  lightsource.\r\n" +
                $"- An entryway (try Doors or Platforms)";
            texture = null;
        }
    }
}
