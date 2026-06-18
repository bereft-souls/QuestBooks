using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class QueenBee : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedQueenBee;
    }
}
