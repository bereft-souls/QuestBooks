namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class WallOfFleshDefeated : VanillaQuest
    {
        public override bool CheckCompletion() => Main.hardMode;
    }
}
