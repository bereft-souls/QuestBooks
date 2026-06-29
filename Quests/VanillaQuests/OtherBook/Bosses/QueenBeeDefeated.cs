namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class QueenBeeDefeated : VanillaQuest
    {
        public override bool CheckCompletion() => NPC.downedQueenBee;
    }
}
