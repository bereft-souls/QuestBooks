using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class FirstSteps : Quest
    {
        public override bool CheckCompletion()
        {
            return true;
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "First Steps";
            contents = $"Welcome to the wonderful, yet cruel world of Terraria!" +
                $"\n" +
                $"Explore diverse biomes, face off dangerous foes, loot structures and craft weapons all to aid you in your journey!";
            texture = null;
        }
    }
}
