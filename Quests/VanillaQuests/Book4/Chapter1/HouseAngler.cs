namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseAngler : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Angler);
}