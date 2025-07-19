using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class EyeOfCthulhuDefeated : Quest
    {
        public override bool HasInfoPage => true;

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "The Eye of Cthulhu";
            contents = $"Defeat the Eye of Cthulhu\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"Summoned by:\n" +
                $"Using a [i:{ItemID.SuspiciousLookingEye}] at night";
            texture = null;
        }

        public override bool CheckCompletion() => NPC.downedBoss1;
    }
}
