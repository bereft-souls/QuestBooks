namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class DeerclopsDefeated : VanillaQuest
    {
        public override bool CheckCompletion() => NPC.downedDeerclops;
    }
}
