using Microsoft.Xna.Framework.Graphics;


namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class CraftWorkbench : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: check for player crafting / acquiring workbench, achievement exists aswell
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Benched";
            contents = "Using the wood you collected, craft an all-purpose Workbench to unlock more recipes!\n" +
                "(To view the new recipes, you must stand closeby to the station)"; // Should change for 1.4.5's crafting UIs
            texture = null;
        }
    }
}
