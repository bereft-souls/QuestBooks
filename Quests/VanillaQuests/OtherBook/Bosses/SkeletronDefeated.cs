namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class SkeletronDefeated : VanillaQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss3;
    }
}
