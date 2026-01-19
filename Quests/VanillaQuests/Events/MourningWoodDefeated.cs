using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    internal class MourningWoodDefeated : Quest
    {
        public override bool CheckCompletion()
        {
            return NPC.downedHalloweenTree;
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}
