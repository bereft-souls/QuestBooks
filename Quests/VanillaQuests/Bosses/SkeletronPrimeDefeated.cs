using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class SkeletronPrimeDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedMechBoss3;
    }
}
