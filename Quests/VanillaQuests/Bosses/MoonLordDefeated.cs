using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class MoonLordDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedMoonlord;
    }
}
