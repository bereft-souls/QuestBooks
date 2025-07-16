using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class DestroyerDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedMechBoss1;
    }
}
