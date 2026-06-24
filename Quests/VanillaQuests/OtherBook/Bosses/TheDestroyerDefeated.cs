namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class TheDestroyerDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss1;
}
