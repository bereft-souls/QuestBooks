using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class QueenSlimeDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedQueenSlime;
    }
}
