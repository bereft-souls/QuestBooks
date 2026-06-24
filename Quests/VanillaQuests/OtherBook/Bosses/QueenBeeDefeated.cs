namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class QueenBeeDefeated : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedQueenBee;
    }
}
