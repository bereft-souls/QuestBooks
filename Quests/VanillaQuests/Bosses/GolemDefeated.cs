using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class GolemDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedGolemBoss;
    }
}
