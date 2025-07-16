using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class SkeletronDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedBoss3;
    }
}
