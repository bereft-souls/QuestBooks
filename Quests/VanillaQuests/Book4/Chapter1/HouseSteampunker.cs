namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseSteampunker : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Steampunker);
}