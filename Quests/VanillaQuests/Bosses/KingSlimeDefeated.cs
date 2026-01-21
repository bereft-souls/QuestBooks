using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class KingSlimeDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedSlimeKing;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = Language.GetTextValue("Mods.QuestBooks.Questbooks.Bosses.KingSlime.Title");
            contents = Language.GetTextValue("Mods.QuestBooks.QuestBooks.Bosses.KingSlime.Contents");
            texture = null;
        }
    }
}
