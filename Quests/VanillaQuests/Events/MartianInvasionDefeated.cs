using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    public class MartianInvasionDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return NPC.downedMartians;
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "";     // TODO: Desc
            contents = "";
            texture = null;
        }
    }
}
