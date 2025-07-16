using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class QueenBeeDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedQueenBee;
    }
}
