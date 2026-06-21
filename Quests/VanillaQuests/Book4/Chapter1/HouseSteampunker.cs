namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseSteampunker : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Steampunker);
}