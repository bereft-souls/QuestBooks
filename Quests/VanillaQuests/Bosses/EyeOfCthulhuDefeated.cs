using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class EyeOfCthulhuDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedBoss1;
    }
}
