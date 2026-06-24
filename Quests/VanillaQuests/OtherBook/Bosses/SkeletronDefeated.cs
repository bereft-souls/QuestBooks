namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class SkeletronDefeated : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss3;
    }
}
