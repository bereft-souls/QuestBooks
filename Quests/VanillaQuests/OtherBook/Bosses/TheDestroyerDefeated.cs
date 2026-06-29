namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class TheDestroyerDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss1;
}
