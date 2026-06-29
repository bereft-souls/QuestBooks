namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class GolemDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedGolemBoss;
}
