namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class EyeOfCthulhuDefeated : VanillaQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss1;
    }
}
