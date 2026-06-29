namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatAnyMech : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBossAny;
}