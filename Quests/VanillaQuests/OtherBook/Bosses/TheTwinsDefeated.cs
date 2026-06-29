namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class TheTwinsDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss2;
}
