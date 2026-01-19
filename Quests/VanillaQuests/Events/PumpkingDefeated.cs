using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    internal class PumpkingDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return NPC.downedHalloweenKing;
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
