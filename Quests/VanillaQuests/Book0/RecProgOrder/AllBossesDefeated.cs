using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.RecProgOrder
{
    public class AllBossesDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // check for ALL bosses being defeated, unlike the achievement this INCLUDES the 1.4 bosses (QS, EoL and DClops).
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Slayer of Worlds";
            contents = "Defeat every boss in Terraria."; // maybe fatten the flavour, this is just the achievement text
            texture = null;
        }
    }
}
